
$(function () {

    var $zipCodes = $('.zip-codes'),
        $normalizedZipCodes = $('.normalized-zip-codes');

    function getZipCodes($container, convertData, callback) {
        var url = $container.data('url'),
            $zipCodeBody = $container.find('tbody'),
            start = new Date().getTime();

        $.ajax({
            type: 'GET',
            url: url,
            success: function (data, textstatus, request) {

                var firstTransfer = new Date().getTime();

                $('.dataSize', $container).text(Number(request.getResponseHeader("Content-Length") / 1024).toFixed(2));
                $('.transfer', $container).text((firstTransfer - start) / 1000);

                //Hit the Server Twice to get Metrics on server calls w/ and w/o Server Caching.
                $.ajax({
                    type: 'GET',
                    url: url,
                    success: function (data, textstatus, request) {

                        var processingStart = new Date().getTime(),
                        zipCodes = convertData(data);

                        $('.serverCache', $container).text((processingStart - firstTransfer) / 1000);

                        for (var i = 0; i < zipCodes.length; i++) {
                            var zipCode = zipCodes[i];

                            $zipCodeBody.append('<tr><td>' + (i + 1) + '</td><td>' +
                                zipCode.ZipCode + '</td><td>' +
                                zipCode.City + '</td><td>' +
                                zipCode.State + '</td><td>' +
                                zipCode.Population + '</td><td>' +
                                //zipCode.LatitudeLongitude[0] + ', ' + zipCode.LatitudeLongitude[1] +
                                '</td></tr>');
                        }

                        var processingEnd = new Date().getTime();

                        $('.processing', $container).text((processingEnd - processingStart) / 1000);

                        if (callback) {
                            window.setTimeout(callback, 1);
                        }
                    }
                });

            }
        });
    }

    getZipCodes($zipCodes, function (data) { return data; }, function () {
        getZipCodes($normalizedZipCodes, function (data) { return normaleezie.denormalize(data); });
    });

});