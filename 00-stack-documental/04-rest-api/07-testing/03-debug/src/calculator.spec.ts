import * as jwt from 'jsonwebtoken';
import * as calculator from './calculator.js';
import * as business from './business/index.js';

vi.mock('jsonwebtoken', async (importOriginal) => {
  const original: any = await importOriginal();
  return {
    ...original.default,
    __esModule: true,
  };
});

describe('Calculator specs', () => {
  describe('add', () => {
    it('should return 4 when passing A equals 2 and B equals 2', () => {
      // Arrange
      const a = 2;
      const b = 2;

      // Act
      const result = calculator.add(a, b);

      // Assert
      expect(result).toEqual(4);
    });

    it('should call to isLowerThan when passing A equals 2 and B equals 2', () => {
      // Arrange
      const a = 2;
      const b = 2;
      vi.spyOn(business, 'isLowerThan').mockImplementation((result) =>
        console.log(`This is the result ${result}`)
      );
      vi.spyOn(business, 'max', 'get').mockReturnValue(7);
      vi.spyOn(jwt, 'sign').mockImplementation((result) => {
        console.log(`Sign result ${result}`);
      });

      // Act
      const result = calculator.add(a, b);

      // Assert
      expect(business.isLowerThan).toHaveBeenCalled();
      expect(business.isLowerThan).toHaveBeenCalledWith(4, 7);
      expect(jwt.sign).toHaveBeenCalledWith(4, 'my-secret');
    });

    it('should call to original implementation isLowerThan', () => {
      // Arrange
      const a = 1;
      const b = 2;

      // Act
      const result = calculator.add(a, b);

      // Assert
      expect(result).toEqual(3);
    });
  });
});
