using System;
using System.Numerics;
using Godot;

namespace RoseIsland.CustomClass
{
    /// <summary>
    /// By representing matrices through arrays, a method is provided to establish the connection between Vector2I in the GODOT library and arrays. This approach facilitates the construction of the necessary relationships between these components.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Matrix<T> where T : struct, INumber<T>
    {
        private T[,] _matrix;

        public Matrix(int size)
        {
            _matrix = new T[size, size];
        }

        public Matrix(T[,] matrix)
        {
            _matrix = matrix;
        }

        /// <summary>
        /// Return the size of the matrix.
        /// </summary>
        public int GetSize() => _matrix.GetLength(0);

        /// <summary>
        /// Set the value of a cell through a Vector2I.
        /// </summary>
        public void SetValue(Vector2I cell, T value)
        {
            _matrix[cell.X, cell.Y] = value;
        }

        /// <summary>
        /// Return the value of a cell through a Vector2I.
        /// </summary>
        public T GetValue(Vector2I cell) => _matrix[cell.X, cell.Y];

        /// <summary>
        /// Return a 2D array through the matrix.
        /// </summary>
        /// <returns></returns>
        public T[,] GetMatrix()
        {
            return _matrix;
        }

        public void Clear()
        {
            int size = this.GetSize();

            _matrix = new T[size, size];
        }

        /// <summary>
        /// Add two matrices together.
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public Matrix<T> AddMatrix(Matrix<T> matrix)
        {
            if (this.GetSize() != matrix.GetSize())
                throw new ArgumentException("Matrices must be of the same size.");

            int size = GetSize();
            T[,] result = new T[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    result[i, j] = _matrix[i, j] + matrix._matrix[i, j];
                }
            }

            return new Matrix<T>(result);
        }

        /// <summary>
        /// Subtract two matrices.
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public Matrix<T> SubtractMatrix(Matrix<T> matrix)
        {
            if (this.GetSize() != matrix.GetSize())
                throw new ArgumentException("Matrices must be of the same size.");

            int size = GetSize();
            T[,] result = new T[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    result[i, j] = _matrix[i, j] - matrix._matrix[i, j];
                }
            }

            return new Matrix<T>(result);
        }

        /// <summary>
        /// Multiply two matrices.
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public Matrix<T> MultiplyMatrix(Matrix<T> matrix)
        {
            if (this.GetSize() != matrix.GetSize())
                throw new ArgumentException("Matrices must be of the same size.");

            int size = GetSize();
            T[,] result = new T[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    T sum = default;
                    for (int k = 0; k < size; k++)
                    {
                        sum += _matrix[i, k] * matrix._matrix[k, j];
                    }
                    result[i, j] = sum;
                }
            }

            return new Matrix<T>(result);
        }

        /// <summary>
        /// Divide two matrices.
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public Matrix<T> DivideMatrix(Matrix<T> matrix)
        {
            if (this.GetSize() != matrix.GetSize())
                throw new ArgumentException("Matrices must be of the same size.");

            int size = GetSize();
            T[,] result = new T[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    result[i, j] = _matrix[i, j] / matrix._matrix[i, j];
                }
            }

            return new Matrix<T>(result);
        }
        
        /// <summary>
        /// Multiply a matrix by a scalar.
        /// </summary>
        /// <param name="scalar"></param>
        /// <returns></returns>
        public Matrix<T> ScalarMultiply(T scalar)
        {
            int size = GetSize();
            T[,] r = new T[size, size];
    
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    r[i, j] = _matrix[i, j] * scalar;
                }
            }
            
            return new Matrix<T>(r);
        }

        /// <summary>
        /// Transpose a matrix.
        /// </summary>
        /// <returns></returns>
        public Matrix<T> Transpose()
        {
            int size = GetSize();
            T[,] result = new T[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    result[i, j] = _matrix[j, i];
                }
            }

            return new Matrix<T>(result);
        }

        /// <summary>
        /// Check if two matrices are equal.
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public bool IsEqual(Matrix<T> matrix)
        {
            if (this.GetSize() != matrix.GetSize()) 
                return false;

            int size = GetSize();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (_matrix[i, j] != matrix._matrix[i, j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}