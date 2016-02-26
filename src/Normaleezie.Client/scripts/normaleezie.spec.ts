﻿/// <reference path="../assets/jasmine.d.ts" />
/// <reference path="normaleezie.ts" />

describe('Normaleezie', () => {

    it('should include a namespace', () => {
        expect(normaleezie).not.toBeUndefined();
        expect(normaleezie).not.toBeNull();
    });

    it('should include a denormalize function', () => {
        expect(normaleezie.denormalize).not.toBeUndefined();
        expect(normaleezie.denormalize).not.toBeNull();

        expect(typeof (normaleezie.denormalize)).toEqual("function");
    });

    describe('when calling denormalize with no parameters', () => {

        var returnValue : any;

        beforeEach(() => {
            returnValue = (<any>normaleezie.denormalize)();
        });

        it('should return undefined', () => {
            expect(returnValue).toBeUndefined();
        });

    });

    describe('when calling denormalize with null', () => {

        var returnValue: any;

        beforeEach(() => {
            returnValue = normaleezie.denormalize(null);
        });

        it('should return null', () => {
            expect(returnValue).toBeNull();
        });

    });

    describe('when calling denormalize with undefined', () => {

        var returnValue: any;

        beforeEach(() => {
            returnValue = normaleezie.denormalize(undefined);
        });

        it('should return undefined', () => {
            expect(returnValue).toBeUndefined();
        });

    });

    describe('when calling denormalize with a serialized normalized form array', () => {

        var returnValue: any,
            json: string = '[[["AnimalId"],["Age",10,11,2,15],["Name","Tony","Lenny","John","Garry","Zachary"],["Type","Tiger","Giraffe","Zebra"]],[[101,1,1,1],[102,2,2,1],[103,3,3,1],[104,4,1,2],[105,1,4,2],[106,1,5,3]]]';
        
        beforeEach(() => {
            returnValue = normaleezie.denormalize(json);
        });

        it('should return an array', () => {
            expect(Array.isArray(returnValue)).toBeTruthy();
        });

        it("should equal the object as if it wasn't normalized", () => {

            var expectedValue = JSON.parse('[{"AnimalId":101,"Age":10,"Name":"Tony","Type":"Tiger"},{"AnimalId":102,"Age":11,"Name":"Lenny","Type":"Tiger"},{"AnimalId":103,"Age":2,"Name":"John","Type":"Tiger"},{"AnimalId":104,"Age":15,"Name":"Tony","Type":"Giraffe"},{"AnimalId":105,"Age":10,"Name":"Garry","Type":"Giraffe"},{"AnimalId":106,"Age":10,"Name":"Zachary","Type":"Zebra"}]');

            expect(returnValue).toEqual(expectedValue);
        });
    });


    describe('when calling denormalize with a normalized form array', () => {

        var returnValue: any,
            json: string = '[[["AnimalId"],["Age",10,11,2,15],["Name","Tony","Lenny","John","Garry","Zachary"],["Type","Tiger","Giraffe","Zebra"]],[[101,1,1,1],[102,2,2,1],[103,3,3,1],[104,4,1,2],[105,1,4,2],[106,1,5,3]]]',
            normalizedArray: Array<any> = JSON.parse(json);

        beforeEach(() => {
            returnValue = normaleezie.denormalize(normalizedArray);
        });

        it('should return an array', () => {
            expect(Array.isArray(returnValue)).toBeTruthy();
        });

        it("should equal the object as if it wasn't normalized", () => {

            var expectedValue = JSON.parse('[{"AnimalId":101,"Age":10,"Name":"Tony","Type":"Tiger"},{"AnimalId":102,"Age":11,"Name":"Lenny","Type":"Tiger"},{"AnimalId":103,"Age":2,"Name":"John","Type":"Tiger"},{"AnimalId":104,"Age":15,"Name":"Tony","Type":"Giraffe"},{"AnimalId":105,"Age":10,"Name":"Garry","Type":"Giraffe"},{"AnimalId":106,"Age":10,"Name":"Zachary","Type":"Zebra"}]');

            expect(returnValue).toEqual(expectedValue);
        });
    });


    describe('when calling denormalize with a normalized form array of books', () => {

        var returnValue: any,
            json: string = '[[["Title"],["Author","J.R.R. Tolkien","Jim Butcher"],["PublishDate"],["Series","The Lord of the Rings","The Dresden Files"],["PurchaseLocation","Barnes and Noble","Amazon"],["PurchaseYear",2000,2015,2016],["HasRead",true,false]],[["The Fellowship of the Ring",1,"1954-07-29T00:00:00",1,1,1,1],["The Two Towers",1,"1954-11-11T00:00:00",1,1,1,1],["The The Return of the King",1,"1955-10-20T00:00:00",1,1,1,1],["Storm Front",2,"2000-04-01T00:00:00",2,2,2,1],["Fool Moon",2,"2001-01-01T00:00:00",2,2,2,1],["Grave Peril",2,"2001-09-01T00:00:00",2,2,2,1],["Summer Knight",2,"2002-02-02T00:00:00",2,2,2,1],["Death Masks",2,"2003-08-05T00:00:00",2,2,2,1],["Blood Rites",2,"2004-08-02T00:00:00",2,2,2,1],["Dead Beat",2,"2005-05-03T00:00:00",2,2,2,1],["Proven Guilty",2,"2006-05-02T00:00:00",2,2,2,1],["White Night",2,"2007-04-03T00:00:00",2,2,2,1],["Small Favor",2,"2008-04-01T00:00:00",2,2,3,1],["Turn Coat",2,"2009-04-07T00:00:00",2,2,3,1],["Changes",2,"2010-04-06T00:00:00",2,2,3,2],["Ghost Story",2,"2011-04-26T00:00:00",2,2,3,2],["Cold Days",2,"2012-11-27T00:00:00",2,2,3,2],["Skin Game",2,"2014-05-27T00:00:00",2,2,3,2]]]',
            normalizedArray: Array<any> = JSON.parse(json);

        beforeEach(() => {
            returnValue = normaleezie.denormalize(normalizedArray);
        });

        it('should return an array', () => {
            expect(Array.isArray(returnValue)).toBeTruthy();
        });

        it("should equal the object as if it wasn't normalized", () => {

            var expectedValue = JSON.parse('[{"Title":"The Fellowship of the Ring","Author":"J.R.R. Tolkien","PublishDate":"1954-07-29T00:00:00","Series":"The Lord of the Rings","PurchaseLocation":"Barnes and Noble","PurchaseYear":2000,"HasRead":true},{"Title":"The Two Towers","Author":"J.R.R. Tolkien","PublishDate":"1954-11-11T00:00:00","Series":"The Lord of the Rings","PurchaseLocation":"Barnes and Noble","PurchaseYear":2000,"HasRead":true},{"Title":"The The Return of the King","Author":"J.R.R. Tolkien","PublishDate":"1955-10-20T00:00:00","Series":"The Lord of the Rings","PurchaseLocation":"Barnes and Noble","PurchaseYear":2000,"HasRead":true},{"Title":"Storm Front","Author":"Jim Butcher","PublishDate":"2000-04-01T00:00:00","Series":"The Dresden Files","PurchaseLocation":"Amazon","PurchaseYear":2015,"HasRead":true},{"Title":"Fool Moon","Author":"Jim Butcher","PublishDate":"2001-01-01T00:00:00","Series":"The Dresden Files","PurchaseLocation":"Amazon","PurchaseYear":2015,"HasRead":true},{"Title":"Grave Peril","Author":"Jim Butcher","PublishDate":"2001-09-01T00:00:00","Series":"The Dresden Files","PurchaseLocation":"Amazon","PurchaseYear":2015,"HasRead":true},{"Title":"Summer Knight","Author":"Jim Butcher","PublishDate":"2002-02-02T00:00:00","Series":"The Dresden Files","PurchaseLocation":"Amazon","PurchaseYear":2015,"HasRead":true},{"Title":"Death Masks","Author":"Jim Butcher","PublishDate":"2003-08-05T00:00:00","Series":"The Dresden Files","PurchaseLocation":"Amazon","PurchaseYear":2015,"HasRead":true},{"Title":"Blood Rites","Author":"Jim Butcher","PublishDate":"2004-08-02T00:00:00","Series":"The Dresden Files","PurchaseLocation":"Amazon","PurchaseYear":2015,"HasRead":true},{"Title":"Dead Beat","Author":"Jim Butcher","PublishDate":"2005-05-03T00:00:00","Series":"The Dresden Files","PurchaseLocation":"Amazon","PurchaseYear":2015,"HasRead":true},{"Title":"Proven Guilty","Author":"Jim Butcher","PublishDate":"2006-05-02T00:00:00","Series":"The Dresden Files","PurchaseLocation":"Amazon","PurchaseYear":2015,"HasRead":true},{"Title":"White Night","Author":"Jim Butcher","PublishDate":"2007-04-03T00:00:00","Series":"The Dresden Files","PurchaseLocation":"Amazon","PurchaseYear":2015,"HasRead":true},{"Title":"Small Favor","Author":"Jim Butcher","PublishDate":"2008-04-01T00:00:00","Series":"The Dresden Files","PurchaseLocation":"Amazon","PurchaseYear":2016,"HasRead":true},{"Title":"Turn Coat","Author":"Jim Butcher","PublishDate":"2009-04-07T00:00:00","Series":"The Dresden Files","PurchaseLocation":"Amazon","PurchaseYear":2016,"HasRead":true},{"Title":"Changes","Author":"Jim Butcher","PublishDate":"2010-04-06T00:00:00","Series":"The Dresden Files","PurchaseLocation":"Amazon","PurchaseYear":2016,"HasRead":false},{"Title":"Ghost Story","Author":"Jim Butcher","PublishDate":"2011-04-26T00:00:00","Series":"The Dresden Files","PurchaseLocation":"Amazon","PurchaseYear":2016,"HasRead":false},{"Title":"Cold Days","Author":"Jim Butcher","PublishDate":"2012-11-27T00:00:00","Series":"The Dresden Files","PurchaseLocation":"Amazon","PurchaseYear":2016,"HasRead":false},{"Title":"Skin Game","Author":"Jim Butcher","PublishDate":"2014-05-27T00:00:00","Series":"The Dresden Files","PurchaseLocation":"Amazon","PurchaseYear":2016,"HasRead":false}]');

            expect(returnValue).toEqual(expectedValue);
        });
    });
    
});