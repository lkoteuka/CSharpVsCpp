// Matrix_CPP.cpp: определяет экспортированные функции для приложения DLL.
//

#include "stdafx.h"
#include <time.h>

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

		double F, G, R, S, T;

		x_buff[0] = 1 / row[0];
		y_buff[0] = 1 / row[0];

		for (int j = 1; j < n; j++)
		{
			F = 0; G = 0;
			for (int i = 0; i < j; i++)
			{
				//вычисляем коэффициенты F и G
				F += column[j - i] * x_buff[i];
				G += row[i + 1] * y_buff[i];
			}

			R = 1 / (1 - F * G);
			S = -R * F;
			T = -R * G;

			x[0] = x_buff[0] * R;        //вычисляем отдельно первые и последние коэффициенты 
			x[j] = y_buff[j - 1] * S;    //для оптимизации цикла

			y[0] = x_buff[0] * T;
			y[j] = y_buff[j - 1] * R;

			for (int d = 1; d < j; d++)
			{
				x[d] = x_buff[d] * R + y_buff[d - 1] * S;
				y[d] = x_buff[d] * T + y_buff[d - 1] * R;
			}

			for (int d = 0; d <= j; d++)
			{
				x_buff[d] = x[d];
				y_buff[d] = y[d];
			}
		}
		//находим решение res[]
		for (int j = 0; j < n; j++)
		{
			res[j] = 0;
			for (int i = n - j - 1; i < n - 1; i++)
			{
				//System.Console.Write(y[i] + " ");
				res[j] += y[i];
			}

			for (int i = 0; i < n - j; i++)
			{
				//System.Console.Write(x[i] + " ");
				res[j] += x[i];
			}
			res[j] *= rh[j];
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
