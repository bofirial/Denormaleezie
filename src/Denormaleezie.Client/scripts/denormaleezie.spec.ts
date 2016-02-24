/// <reference path="../assets/jasmine.d.ts" />
/// <reference path="denormaleezie.ts" />

describe('Denormaleezie', () => {

    it('should include a namespace', () => {
        expect(denormaleezie).not.toBeUndefined();
        expect(denormaleezie).not.toBeNull();
    });

    it('should include a denormalize function', () => {
        expect(denormaleezie.denormalize).not.toBeUndefined();
        expect(denormaleezie.denormalize).not.toBeNull();

        expect(typeof(denormaleezie.denormalize)).toEqual("function");
    });

    describe('when calling denormalize with no parameters', () => {

        var returnValue : any;

        beforeEach(() => {
            returnValue = (<any>denormaleezie.denormalize)();
        });

        it('should return null', () => {
            expect(returnValue).toBeUndefined();
        });

    });

    describe('when calling denormalize with null', () => {

        var returnValue: any;

        beforeEach(() => {
            returnValue = denormaleezie.denormalize(null);
        });

        it('should return null', () => {
            expect(returnValue).toBeNull();
        });

    });

    describe('when calling denormalize with undefined', () => {

        var returnValue: any;

        beforeEach(() => {
            returnValue = denormaleezie.denormalize(undefined);
        });

        it('should return null', () => {
            expect(returnValue).toBeUndefined();
        });

    });
});