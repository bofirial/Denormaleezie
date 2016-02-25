
namespace denormaleezie {
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
            var listItem = {};

            for (var i = 0; i < denormalizedData.length; i++) {
                var prop = denormalizedData[i],
                    value = structureItem[i];

                if (prop.length > 1)
                {
                    value = prop[structureItem[i]];
                }

                listItem[prop[0]] = value;
            }

            normalizedList.push(listItem);
        }

        return normalizedList;
    }
}