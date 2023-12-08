using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode.days;

public class Day8 : Day
{
    public Day8() : base(8)
    {
    }


    protected override void Run(bool isPart2 = false)
    {
        var actions = Input[0].ToCharArray();

        var nodes = new Dictionary<string, (string, string)>();

        for (var i = 2; i < Input.Length; i++)
        {
            //RTF = (TRM, KNP)
            var parts = Input[i].Split(' ');
            nodes.Add(parts[0],
                (Regex.Replace(parts[2], "[(,]", "").Replace("(", "").Replace(",", ""), parts[3].Replace(")", "")));
        }

        // start at the root node
        var currentNode = "AAA";

        var total = 0;
        var actionIndex = 0;

        if (isPart2) goto Part2;
        while (currentNode != "ZZZ")
        {
            var action = actions[actionIndex];
            if (action == 'L')
                currentNode = nodes[currentNode].Item1;
            else
                currentNode = nodes[currentNode].Item2;
            actionIndex++;
            if (actionIndex == actions.Length) actionIndex = 0;
            total++;
        }

        Console.WriteLine(total);
        return;
        Part2:
        total = 0;

        var currentNodes = new List<string>();

        foreach (var nodesKey in nodes.Keys)
            if (nodesKey.EndsWith("A"))
                currentNodes.Add(nodesKey);
        var allNodesAreZ = false;
        
        var tmpNodes = new List<string>();
        while (!allNodesAreZ)
        {
            allNodesAreZ = true;
            var action = actions[actionIndex];
            if (action == 'L')
                currentNodes.ForEach(node => tmpNodes.Add(nodes[node].Item1));
            else
                currentNodes.ForEach(node => tmpNodes.Add(nodes[node].Item2));
            actionIndex++;
            if (actionIndex == actions.Length) actionIndex = 0;
            total++;
            currentNodes = tmpNodes;
            tmpNodes = new List<string>();
            // check if all nodes are Z
            currentNodes.ForEach(node => allNodesAreZ = node.EndsWith("Z") && allNodesAreZ);
        }
        
        Console.WriteLine(total);
    }
}