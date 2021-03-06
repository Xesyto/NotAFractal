﻿using System;
using System.Collections.Generic;
using NotAFractal.Models.ViewModels;

namespace NotAFractal.Models.Builders
{
    public class NodeViewModelBuilder
    {
        public NodeViewModel Build(int seed, FractalNode type)
        {
            var random = new Random(seed);
            var modelManager = ModelManager.Instance;
            var dataGeneratorSeed = random.Next(1, int.MaxValue);

            var nodeViewModel = new NodeViewModel
            {
                Title = modelManager.ProcessDataGeneratorSymbols(dataGeneratorSeed, type.Title),
                Name = modelManager.ProcessDataGeneratorSymbols(dataGeneratorSeed, type.Name),
                Text = modelManager.ProcessDataGeneratorSymbols(dataGeneratorSeed, type.Text),

                Seed = seed,
                Type = type.Type,
            };


            if (type.ChoiceNodes != null)
            {
                foreach (var choiceNode in type.PickChoiceNodes(random.Next(1, int.MaxValue)))
                {
                    nodeViewModel.ChildNodes.Add(modelManager.BuildNodeViewModelStub(random.Next(1, int.MaxValue), choiceNode));
                }
            }

            if (type.Nodes == null)
                return nodeViewModel;

            foreach (var weightedNode in type.Nodes)
            {
                if(random.Next(0,100) <=  weightedNode.PercentageChance)
                {
                    var count = random.Next(weightedNode.MinAmount, weightedNode.MaxAmount);
                    
                    while( count > 0)
                    {
                        if( nodeViewModel.ChildNodes == null)
                            nodeViewModel.ChildNodes = new List<NodeViewModel>();
                        
                        nodeViewModel.ChildNodes.Add(modelManager.BuildNodeViewModelStub(random.Next(1,int.MaxValue),weightedNode.Type));
                        count--;
                    }
                }
            }

            return nodeViewModel;
        }

        public NodeViewModel BuildStub(int seed, FractalNode type)
        {
            var random = new Random(seed);
            var modelManager = ModelManager.Instance;
            var dataGeneratorSeed = random.Next(1, int.MaxValue);

            var nodeViewModel = new NodeViewModel
            {
                Title = modelManager.ProcessDataGeneratorSymbols(dataGeneratorSeed, type.Title),

                Seed = seed,
                Type = type.Type,
            };

            if( string.IsNullOrEmpty(type.Name) &&
                string.IsNullOrEmpty(type.Text) &&
                type.Nodes == null && 
                type.ChoiceNodes == null)
            {
                nodeViewModel.EmptyNode = true;
            }

            return nodeViewModel;
        }

        public NodeViewModel ExceptionNode(string message)
        {            
            var nodeViewModel = new NodeViewModel
            {
                Title = message,
                Seed = 1,
                Type = "ExceptionNode",
            };

            return nodeViewModel;
        }
    }
}