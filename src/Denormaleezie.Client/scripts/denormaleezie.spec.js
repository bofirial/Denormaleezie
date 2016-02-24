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
    describe('when calling denormalize with a serialized denormalized array', function () {
        var returnValue, json = '[[["AnimalId"],["Age",10,11,2,15],["Name","Tony","Lenny","John","Garry","Zachary"],["Type","Tiger","Giraffe","Zebra"]],[[101,1,1,1],[102,2,2,1],[103,3,3,1],[104,4,1,2],[105,1,4,2],[106,1,5,3]]]';
        beforeEach(function () {
            returnValue = denormaleezie.denormalize(json);
        });
        it('should return an array', function () {
            expect(Array.isArray(returnValue)).toBeTruthy();
        });
    });
    describe('when calling denormalize with a denormalized array', function () {
        var returnValue, json = '[[["AnimalId"],["Age",10,11,2,15],["Name","Tony","Lenny","John","Garry","Zachary"],["Type","Tiger","Giraffe","Zebra"]],[[101,1,1,1],[102,2,2,1],[103,3,3,1],[104,4,1,2],[105,1,4,2],[106,1,5,3]]]', denormalizedArray = JSON.parse(json);
        beforeEach(function () {
            returnValue = denormaleezie.denormalize(denormalizedArray);
        });
        it('should return an array', function () {
            expect(Array.isArray(returnValue)).toBeTruthy();
        });
    });
});
