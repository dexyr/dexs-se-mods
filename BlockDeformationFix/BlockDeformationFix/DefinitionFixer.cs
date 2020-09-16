using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sandbox.Definitions;
using VRage.Game;
using VRage.Game.Components;

namespace BlockDeformationFix
{
    [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]

    public class DefinitionFixer : MySessionComponentBase
    {
        private struct DeformationValues
        {
            public bool Uses { get; }
            public float Ratio { get; }

            public DeformationValues(bool uses, float ratio)
            {
                Uses = uses;
                Ratio = ratio;
            }
        }

        //
        Dictionary<MyDefinitionId, DeformationValues> definitionIdToValues = new Dictionary<MyDefinitionId, DeformationValues>();

        public override void LoadData()
        {
            // this is acting funny for whatever reason
            // MyCubeBlockDefinition definitions = MyDefinitionManager.Static.GetAllDefinitions<MyCubeBlockDefinition>();

            var definitions = MyDefinitionManager.Static.GetAllDefinitions();

            foreach (var definition in definitions)
            {
                if (definition is MyCubeBlockDefinition)
                {
                    // yes there's an easier way to do this casting (see the above comment)
                    var cubeBlock = definition as MyCubeBlockDefinition;

                    var originalValues = new DeformationValues(cubeBlock.UsesDeformation, cubeBlock.DeformationRatio);

                    definitionIdToValues.Add(cubeBlock.Id, originalValues);

                    cubeBlock.UsesDeformation = true;
                    cubeBlock.DeformationRatio = 0.4f;
                }
            }
        }

        public override void SaveData()
        {
            // i haven't actually tested this yet so it may not work
            // ie. return blocks to their default values

            foreach (var pair in definitionIdToValues)
            {
                MyCubeBlockDefinition cubeBlock;

                if (MyDefinitionManager.Static.TryGetCubeBlockDefinition(pair.Key, out cubeBlock))
                {
                    cubeBlock.UsesDeformation = pair.Value.Uses;
                    cubeBlock.DeformationRatio = pair.Value.Ratio;
                }
            }
        }
    }
}
