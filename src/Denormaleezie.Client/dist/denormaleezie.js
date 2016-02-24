var denormaleezie;
(function (denormaleezie) {
    function denormalize(json) {
        if (!json) {
            return json;
        }
        return { test: true };
    }
    denormaleezie.denormalize = denormalize;
})(denormaleezie || (denormaleezie = {}));
