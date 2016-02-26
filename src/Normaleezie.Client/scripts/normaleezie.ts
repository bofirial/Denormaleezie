
namespace normaleezie {

    function createDenormalizedItem(normalizedDataList: Array<Array<any>>, normalizedStructureItem : Array<any>) : any {

        var denormalizedItem = {};

        for (var i = 0; i < normalizedDataList.length; i++) {

            var normalizedPropertyData = normalizedDataList[i],
                propName = normalizedPropertyData[0],
                propValue = normalizedStructureItem[i];

            if (normalizedPropertyData.length > 1) {
                propValue = normalizedPropertyData[normalizedStructureItem[i]];
            }

            denormalizedItem[propName] = propValue;
        }

        return denormalizedItem;
    }

    function getNormalizedForm(param: any): Array<Array<Array<any>>> {
        var normalizedForm: Array<Array<Array<any>>> = param;

        if (typeof (param) === 'string') {
            normalizedForm = JSON.parse(param);
        }

        return normalizedForm;
    }

    export function denormalize(param: any): any {

        if (!param) {
            return param;
        }

        var normalizedForm: Array<Array<Array<any>>> = getNormalizedForm(param);

        var normalizedDataList = normalizedForm[0],
            normalizedStructureList = normalizedForm[1],
            denormalizedList = [];

        for (var normalizedStructureItem of normalizedStructureList) {

            denormalizedList.push(createDenormalizedItem(normalizedDataList, normalizedStructureItem));
        }

        return denormalizedList;
    }
}