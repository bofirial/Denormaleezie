var denormaleezie;
(function (denormaleezie) {
    function normalize(param) {
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
        var denormalizedData = denormalizedObject[0], denormalizedStructure = denormalizedObject[1], normalizedList = [];
        for (var _i = 0, denormalizedStructure_1 = denormalizedStructure; _i < denormalizedStructure_1.length; _i++) {
            var structureItem = denormalizedStructure_1[_i];
            var listItem = {};
            for (var i = 0; i < denormalizedData.length; i++) {
                var prop = denormalizedData[i], value = structureItem[i];
                if (prop.length > 1) {
                    value = prop[structureItem[i]];
                }
                listItem[prop[0]] = value;
            }
            normalizedList.push(listItem);
        }
        return normalizedList;
    }
    denormaleezie.normalize = normalize;
})(denormaleezie || (denormaleezie = {}));
