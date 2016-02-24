var denormaleezie;
(function (denormaleezie) {
    function denormalize(param) {
        if (!param) {
            return param;
        }
        var denormalizedObject;
        if (typeof (param) === 'string') {
            denormalizedObject = JSON.parse(param);
        }
        else {
            denormalizedObject = param;
        }
        var denormalizedData = denormalizedObject[0], denormalizedStructure = denormalizedObject[1];
        return [{ test: true }];
    }
    denormaleezie.denormalize = denormalize;
})(denormaleezie || (denormaleezie = {}));
