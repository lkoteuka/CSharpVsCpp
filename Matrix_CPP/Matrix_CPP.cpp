// Matrix_CPP.cpp: определяет экспортированные функции для приложения DLL.
//

#include "stdafx.h"
#include <time.h>
#include <iostream>

class Matrix
{
public:
	int n;
	double * row;
	double * column;
	double timecpp;

	Matrix(int n, double * row, double *column)
	{
		this->n = n;
		this->row = row;
		this->column = column;
	}

	bool Solve(double *rh, double *res)
	{
		double * x = new double[this->n];
		double * y = new double[this->n];
		double *x_buff = new double[n];
		double *y_buff = new double[n];
		double *buff = new double[n];

		double F, G, R, S, T;

		x[0] = 1 / row[0];
		y[0] = 1 / row[0];

		for (int j = 1; j < n; j++)
		{
			F = 0; G = 0;
			for (int i = 0; i < j; i++)
			{
				//вычисляем коэффициенты F и G
				F += column[j - i] * x[i];
				G += row[i + 1] * y[i];
			}

			R = 1 / (1 - F * G);
			S = -R * F;
			T = -R * G;

			y[j] = y[j - 1] * R;        //вычисляем отдельно первые и последние коэффициенты 
			x[j] = y[j - 1] * S;        //для оптимизации цикла
			for (int i = j - 1; i > 0; --i)
			{
				y[i] = x[i] * T + y[i - 1] * R;
				x[i] = x[i] * R + y[i - 1] * S;
			}
			y[0] = x[0] * T;
			x[0] = x[0] * R;
		}
		//находим решение res[]
		for (int i = 0; i < n; ++i)
		{
			buff[i] = 0;
			for (int j = 0; j < n; ++j)
			{
				int num = i - j;
				if (num < 0)
					buff[i] += x[n + num] * rh[j];
			}
		}

		for (int i = 0; i < n; ++i)
		{
			x_buff[i] = 0;
			for (int j = 0; j < n; ++j)
			{
				int num = i - j;
				if (num > 0)
					x_buff[i] += y[num - 1] * buff[j];
			}
		}

		for (int i = 0; i < n; ++i)
		{
			buff[i] = 0;
			for (int j = 0; j < n; ++j)
			{
				int num = i - j;
				if (num <= 0)
					buff[i] += y[n + num - 1] * rh[j];
			}
		}

		for (int i = 0; i < n; ++i)
		{
			y_buff[i] = 0;
			for (int j = 0; j < n; ++j)
			{
				int num = i - j;
				if (num >= 0)
					y_buff[i] += x[num] * buff[j];
			}
		}

		for (int i = 0; i < n; ++i)
		{
			res[i] = (y_buff[i] - x_buff[i]) / x[0];
		}
		return true;
	}

	~Matrix() {}
};

extern "C"
{
	//GLOBAL FUNCTION
	__declspec(dllexport) void GlobalFunction(
		int n,
		double row[],
		double column[],
		double rh[],
		double answ[],
		double* time
	)
	{
	Matrix * mtr = new Matrix(n, row, column);

	//Calculate the solution time
	clock_t solve_time;
	solve_time = clock();

	mtr->Solve(rh, answ);
	solve_time = clock() - solve_time;

	*time = (double)solve_time / CLOCKS_PER_SEC;
	}
}
