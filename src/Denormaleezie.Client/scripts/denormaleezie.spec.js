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
    describe('when calling denormalize with no parameters', function () {
        var returnValue;
        beforeEach(function () {
            returnValue = denormaleezie.denormalize();
        });
        it('should return null', function () {
            expect(returnValue).toBeUndefined();
        });
    });
    describe('when calling denormalize with null', function () {
        var returnValue;
        beforeEach(function () {
            returnValue = denormaleezie.denormalize(null);
        });
        it('should return null', function () {
            expect(returnValue).toBeNull();
        });
    });
    describe('when calling denormalize with undefined', function () {
        var returnValue;
        beforeEach(function () {
            returnValue = denormaleezie.denormalize(undefined);
        });
        it('should return null', function () {
            expect(returnValue).toBeUndefined();
        });
    });
});
