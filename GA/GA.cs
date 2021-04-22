﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GA
{
    public class GA
    {
        public double MutationRate { get; set; }
        public double CrossoverRate { get; set; }
        public int PopulationSize { get; set; }
        public int GenerationSize { get; set; }
        public int GenomeSize { get; set; }
        public double TotalFitness { get; set; }
        public string FitnessFile { get; set; }
        public Func<double[], double> FitnessFunction { get; set; }

        private Random _rnd;
        private bool _elitism;
        private List<Gen> _thisGeneration;
        private List<Gen> _nextGeneration;
        private List<double> _fittnessTable;
        /// <summary>
        /// Базовый конструктор с заданными параметрами
        /// </summary>
        public GA()
        {
            MutationRate = 0.05;
            CrossoverRate = 0.8;
            PopulationSize = 20;
            GenerationSize = 100;
            _elitism = false;
            Initialize();
        }

        /// <param name="mutationRate">Скорость мутации</param>
        /// <param name="crossoverRate">Кроссинговер</param>
        /// <param name="populationSize">Размер популяции</param>
        /// <param name="generationSize">Размер поколения</param>
        /// <param name="genomeSize">Размер генома</param>
        /// <param name="fitnessFile">Файл для записи</param>
        /// <param name="fitnessFunction">Функция пригодности</param>
        public GA(double mutationRate,
            double crossoverRate,
            int populationSize,
            int generationSize,
            int genomeSize,
            string fitnessFile,
            Func<double[], double> fitnessFunction)
        {
            MutationRate = mutationRate;
            CrossoverRate = crossoverRate;
            PopulationSize = populationSize;
            GenerationSize = generationSize;
            GenomeSize = genomeSize;
            FitnessFile = fitnessFile;
            FitnessFunction = fitnessFunction;
            _elitism = false;
            Initialize();
        }

        private void Initialize()
        {
            _rnd = new Random();
            _thisGeneration = new List<Gen>();
            _nextGeneration = new List<Gen>();
            _fittnessTable = new List<double>();
        }

        /// <summary>
        /// Запуск генетического алгоритма
        /// </summary>
        public void Go()
        {
            if (FitnessFunction == null)
                throw new ArgumentNullException(nameof(FitnessFunction));
            if (GenomeSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(GenomeSize));
            Gen.MutationRate = MutationRate;

            CreateGenomes();
            RankPopulation();

            var sb = new StringBuilder();

            for (int i = 0; i < GenerationSize; i++)
            {
                CreateNextGenerations();
                RankPopulation();
                sb.Append($"{i}, {_thisGeneration[i].Fitness}\n");
            }
            if (FitnessFile != null)
            {
                File.WriteAllText(FitnessFile, sb.ToString());
            }
        }

        public void CreateGenomes()
        {
            for (int i = 0; i < PopulationSize; i++)
            {
                _thisGeneration.Add(new Gen(GenomeSize));
            }
        }

        /// <summary>
        /// Создание следующей поколения
        /// </summary>
        private void CreateNextGenerations()
        {
            _nextGeneration.Clear();
            Gen g = null;
            if (_elitism)
            {
                g = _thisGeneration.Last();
            }

            for (int i = 0; i < PopulationSize; i += 2)
            {
                Gen child1, child2;
                var parent1 = _thisGeneration[RouletteSelection()];
                var parent2 = _thisGeneration[RouletteSelection()];
                if (_rnd.NextDouble() < CrossoverRate)
                {
                    parent1.Crossover(parent2, out child1, out child2);
                }
                else
                {
                    child1 = parent1;
                    child2 = parent2;
                }
                child1.Mutate();
                child2.Mutate();

                _nextGeneration.Add(child1);
                _nextGeneration.Add(child2);
            }
            if (_elitism && g != null)
            {
                _nextGeneration[0] = g;
            }

            _thisGeneration.Clear();
            for (int i = 0; i < PopulationSize; i++)
            {
                _thisGeneration.Add(_nextGeneration[i]);
            }
        }
        
        /// <summary>
        /// После ранжирования всех генов по пригодности используется рулеточный отбор
        /// Это дает большую вероятность отбора, тем кто обладает меньшей приспособленностью
        /// </summary>
        /// <returns></returns>
        private int RouletteSelection()
        {
            var randomFitness = _rnd.NextDouble() * TotalFitness;
            var index = -1;
            var first = 0;
            var last = PopulationSize - 1;
            var mid = (last - first) / 2;

            // Используется ручной бинарный поиск поскольку List<T> работает только с точными значениями
            while (index == -1 && first <= last)
            {
                if (randomFitness < _fittnessTable[mid])
                {
                    last = mid;
                }
                else if (randomFitness > _fittnessTable[mid])
                {
                    first = mid;
                }
                mid = (first + last) / 2;
                if ((last - first) == 1)
                {
                    index = last;
                }
            }
            return index;
        }

        /// <summary>
        /// Ранжирование популяции и сортировка по пригодности
        /// </summary>
        private void RankPopulation()
        {
            TotalFitness = 0;
            for (int i = 0; i < PopulationSize; i++)
            {
                Gen g = _thisGeneration[i];
                g.Fitness = FitnessFunction(g.Genes);
                TotalFitness += g.Fitness;
            }
            _thisGeneration.Sort(new GenComparer());

            double fitness = 0;
            _fittnessTable.Clear();
            for (int i = 0; i < PopulationSize; i++)
            {
                fitness += _thisGeneration[i].Fitness;
                _fittnessTable.Add(fitness);
            }
        }

        public Gen GetBestGen()
        {
            return _thisGeneration.Last();
        }

        public Gen GetWorstGen()
        {
            return _thisGeneration.First();
        }

        public Gen GetNthGenome(int n)
        {
            return _thisGeneration[n];
        }
    }
}