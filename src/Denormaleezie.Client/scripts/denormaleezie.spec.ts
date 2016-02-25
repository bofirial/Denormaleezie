/// <reference path="../assets/jasmine.d.ts" />
/// <reference path="denormaleezie.ts" />

describe('Denormaleezie', () => {

    it('should include a namespace', () => {
        expect(denormaleezie).not.toBeUndefined();
        expect(denormaleezie).not.toBeNull();
    });

    it('should include a normalize function', () => {
        expect(denormaleezie.normalize).not.toBeUndefined();
        expect(denormaleezie.normalize).not.toBeNull();

        expect(typeof (denormaleezie.normalize)).toEqual("function");
    });

    describe('when calling normalize with no parameters', () => {

        var returnValue : any;

        beforeEach(() => {
            returnValue = (<any>denormaleezie.normalize)();
        });

        it('should return null', () => {
            expect(returnValue).toBeUndefined();
        });

    });

    describe('when calling normalize with null', () => {

        var returnValue: any;

        beforeEach(() => {
            returnValue = denormaleezie.normalize(null);
        });

        it('should return null', () => {
            expect(returnValue).toBeNull();
        });

    });

    describe('when calling normalize with undefined', () => {

        var returnValue: any;

        beforeEach(() => {
            returnValue = denormaleezie.normalize(undefined);
        });

        it('should return null', () => {
            expect(returnValue).toBeUndefined();
        });

    });

    describe('when calling normalize with a serialized denormalized array', () => {

        var returnValue: any,
            json: string = '[[["AnimalId"],["Age",10,11,2,15],["Name","Tony","Lenny","John","Garry","Zachary"],["Type","Tiger","Giraffe","Zebra"]],[[101,1,1,1],[102,2,2,1],[103,3,3,1],[104,4,1,2],[105,1,4,2],[106,1,5,3]]]';
        
        beforeEach(() => {
            returnValue = denormaleezie.normalize(json);
        });

        it('should return an array', () => {
            expect(Array.isArray(returnValue)).toBeTruthy();
        });
    });


    describe('when calling normalize with a denormalized array', () => {

        var returnValue: any,
            json: string = '[[["AnimalId"],["Age",10,11,2,15],["Name","Tony","Lenny","John","Garry","Zachary"],["Type","Tiger","Giraffe","Zebra"]],[[101,1,1,1],[102,2,2,1],[103,3,3,1],[104,4,1,2],[105,1,4,2],[106,1,5,3]]]',
            denormalizedArray: Array<any> = JSON.parse(json);

        beforeEach(() => {
            returnValue = denormaleezie.normalize(denormalizedArray);
        });

        it('should return an array', () => {
            expect(Array.isArray(returnValue)).toBeTruthy();
        });

        it("should equal the object as if it wasn't denormalized", () => {

            var expectedValue = JSON.parse('[{"AnimalId":101,"Age":10,"Name":"Tony","Type":"Tiger"},{"AnimalId":102,"Age":11,"Name":"Lenny","Type":"Tiger"},{"AnimalId":103,"Age":2,"Name":"John","Type":"Tiger"},{"AnimalId":104,"Age":15,"Name":"Tony","Type":"Giraffe"},{"AnimalId":105,"Age":10,"Name":"Garry","Type":"Giraffe"},{"AnimalId":106,"Age":10,"Name":"Zachary","Type":"Zebra"}]');

            expect(returnValue).toEqual(expectedValue);
        });
    });
    
});