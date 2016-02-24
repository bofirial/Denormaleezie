/// <reference path="../assets/jasmine.d.ts" />
/// <reference path="denormaleezie.ts" />
describe('Denormaleezie', function () {
    it('should include a namespace', function () {
        expect(denormaleezie).not.toBeUndefined();
        expect(denormaleezie).not.toBeNull();
    });
    it('should include a denormalize function', function () {
        expect(denormaleezie.denormalize).not.toBeUndefined();
        expect(denormaleezie.denormalize).not.toBeNull();
        expect(typeof (denormaleezie.denormalize)).toEqual("function");
    });
});
