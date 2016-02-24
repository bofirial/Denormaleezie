
namespace denormaleezie {
    export function denormalize(json : any) : any {

        if (!json) {
            return json;
        }

        return { test: true };
    }
}