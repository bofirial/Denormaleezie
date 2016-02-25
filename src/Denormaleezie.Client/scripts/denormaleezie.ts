
namespace denormaleezie {

    function createNormalizedObject(denormalizedData : Array<Array<any>>, structureItem : Array<any>) : any {

        var normalizedObject = {};

        for (var i = 0; i < denormalizedData.length; i++) {
            var prop = denormalizedData[i],
                value = structureItem[i];

            if (prop.length > 1) {
                value = prop[structureItem[i]];
            }

            normalizedObject[prop[0]] = value;
        }

        return normalizedObject;
    }

    export function normalize(param: any): any {

        if (!param) {
            return param;
        }

        var denormalizedObject: Array<Array<Array<any>>>;

        if (typeof (param) === 'string') {
            denormalizedObject = JSON.parse(param);
        } else {
            denormalizedObject = param;
        }

        var denormalizedData = denormalizedObject[0],
            denormalizedStructure = denormalizedObject[1],
            normalizedList = [];

        for (var structureItem of denormalizedStructure) {

            normalizedList.push(createNormalizedObject(denormalizedData, structureItem));
        }

        return normalizedList;
    }
}