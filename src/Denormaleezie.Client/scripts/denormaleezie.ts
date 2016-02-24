
namespace denormaleezie {
    export function denormalize(param : any) : any {

        if (!param) {
            return param;
        }

        var denormalizedObject: Array<Array<Array<any>>>;

        if (typeof(param) === 'string') {
            denormalizedObject = JSON.parse(param);
        } else {
            denormalizedObject = param;
        }

        var denormalizedData = denormalizedObject[0],
            denormalizedStructure = denormalizedObject[1];

        return [{ test: true }];
    }
}