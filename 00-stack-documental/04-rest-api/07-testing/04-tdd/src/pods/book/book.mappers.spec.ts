import { ObjectId } from 'mongodb';
import * as model from '#dals/index.js';
import * as apiModel from './book.api-model.js';
import { mapBookListFromApiToModel } from './book.mappers.js';

describe('book.mappers spec', () => {
  describe('mapBookListFromApiToModel', () => {
    it.each<{ bookList: apiModel.Book[] }>([
      { bookList: undefined },
      { bookList: null },
      { bookList: [] },
    ])(
      'should return empty array when it feeds bookList equals $bookList',
      ({ bookList }) => {
        // Arrange

        // Act
        const result = mapBookListFromApiToModel(bookList);

        // Assert
        const expectedResult: model.Book[] = [];
        expect(result).toEqual(expectedResult);
      }
    );

    it('should return one mapped item in array when it feeds bookList with one item', () => {
      // Arrange
      const bookList: apiModel.Book[] = [
        {
          id: '60c20a334bec6a37b08acec9',
          title: 'test-title',
          releaseDate: '2021-07-28T12:30:00',
          author: 'test-author',
        },
      ];

      // Act
      const result: model.Book[] = mapBookListFromApiToModel(bookList);

      // Assert
      expect(result).toEqual([
        {
          _id: new ObjectId('60c20a334bec6a37b08acec9'),
          title: 'test-title',
          releaseDate: new Date('2021-07-28T12:30:00'),
          author: 'test-author',
        },
      ]);
    });
  });
});
