import { ObjectId } from 'mongodb';
import * as model from 'dals';
import * as apiModel from './book.api-model';
import { mapBookListFromApiToModel } from './book.mappers';

describe('pods/book/book.mappers spec', () => {
  describe('mapBookListFromApiToModel', () => {
    it.each<apiModel.Book[]>([undefined, null, []])(
      'should return empty array when it feeds bookList equals %p',
      (bookList: any) => {
        // Arrange

        // Act
        const result: model.Book[] = mapBookListFromApiToModel(bookList);

        // Assert
        expect(result).toEqual([]);
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
