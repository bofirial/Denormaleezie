var normaleezie;
(function (normaleezie) {
    function createDenormalizedItem(normalizedDataList, normalizedStructureItem) {
        var denormalizedItem = {};
        for (var i = 0; i < normalizedDataList.length; i++) {
            var normalizedPropertyData = normalizedDataList[i], propName = normalizedPropertyData[0], propValue = normalizedStructureItem[i];
            if (propName.substr(-1) === '~') {
                propName = propName.substr(0, propName.length - 1);
                propValue = [];
                for (var _i = 0, _a = normalizedStructureItem[i]; _i < _a.length; _i++) {
                    var subNormalizedStructureItem = _a[_i];
                    propValue.push(createDenormalizedItem(normalizedPropertyData.slice(1), subNormalizedStructureItem));
                }
            }
            else if (propName.substr(-1) === '.') {
                propName = propName.substr(0, propName.length - 1);
                propValue = createDenormalizedItem(normalizedPropertyData.slice(1), normalizedStructureItem[i]);
            }
            else if (normalizedPropertyData.length > 1) {
                propValue = normalizedPropertyData[normalizedStructureItem[i]];
            }
            if (propName === "") {
                denormalizedItem = propValue;
            }
            else {
                denormalizedItem[propName] = propValue;
            }
        }
        return denormalizedItem;
    }
    function getNormalizedForm(param) {
        var normalizedForm = param;
        if (typeof (param) === 'string') {
            normalizedForm = JSON.parse(param);
        }
        return normalizedForm;
    }
    function denormalize(param) {
        if (!param) {
            return param;
        }
        var normalizedForm = getNormalizedForm(param);
        var normalizedDataList = normalizedForm[0], normalizedStructureList = normalizedForm[1], denormalizedList = [];
        for (var _i = 0, normalizedStructureList_1 = normalizedStructureList; _i < normalizedStructureList_1.length; _i++) {
            var normalizedStructureItem = normalizedStructureList_1[_i];
            denormalizedList.push(createDenormalizedItem(normalizedDataList, normalizedStructureItem));
        }
        return denormalizedList;
    }
    normaleezie.denormalize = denormalize;
})(normaleezie || (normaleezie = {}));
