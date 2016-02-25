
namespace normaleezie {

    function createDenormalizedObject(normalizedData : Array<Array<any>>, structureItem : Array<any>) : any {

        var denormalizedObject = {};

        for (var i = 0; i < normalizedData.length; i++) {
            var prop = normalizedData[i],
                value = structureItem[i];

            if (prop.length > 1) {
                value = prop[structureItem[i]];
            }

            denormalizedObject[prop[0]] = value;
        }

        return denormalizedObject;
    }

    export function denormalize(param: any): any {

        if (!param) {
            return param;
        }

        var normalizedObject: Array<Array<Array<any>>>;

        if (typeof (param) === 'string') {
            normalizedObject = JSON.parse(param);
        } else {
            normalizedObject = param;
        }

        var normalizedData = normalizedObject[0],
            normalizedStructure = normalizedObject[1],
            denormalizedList = [];

        for (var structureItem of normalizedStructure) {

            denormalizedList.push(createDenormalizedObject(normalizedData, structureItem));
        }

        return denormalizedList;
    }
}