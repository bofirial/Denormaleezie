using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Normaleezie.NormalizedData;
using Normaleezie.NormalizedStructure;

namespace Normaleezie
{
    public class Normalizer
    {
        private readonly NormalizedDataManager _normalizedDataManager;
        private readonly NormalizedStructureManager _normalizedStructureManager;

        public Normalizer() : this(new NormalizedDataManager(), new NormalizedStructureManager()) { }

        internal Normalizer(NormalizedDataManager normalizedDataManager, NormalizedStructureManager normalizedStructureManager)
        {
            this._normalizedDataManager = normalizedDataManager;
            this._normalizedStructureManager = normalizedStructureManager;
        }

        public virtual List<List<List<object>>> Normalize<T>(List<T> denormalizedList)
        {
            if (null == denormalizedList || !denormalizedList.Any())
            {
                return new List<List<List<object>>>();
            }

            return ConvertToNormalizedForm(denormalizedList);
        }

        internal virtual List<List<List<object>>> ConvertToNormalizedForm<T>(List<T> denormalizedList)
        {
            List<List<object>> normalizedDataList = _normalizedDataManager.CreateNormalizedData(denormalizedList);
            List<List<object>> normalizedStructureList = _normalizedStructureManager.CreateNormalizedStructure(denormalizedList, normalizedDataList);

            return new List<List<List<object>>>() {
                normalizedDataList, normalizedStructureList
            };
        }
    }
}
