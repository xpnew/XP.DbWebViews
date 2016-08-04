using FreshGoodBranch.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreshGoodBranch.BLL
{
    public class LevelRightBLL : Base.PredicateBassBLL<N_LevelRight>
    {


        protected override void InitPredicate()
        {
            IdxPredicate = u => u.KeyId == this.CurrentIdx;

            IdxListPredicate = u => IdxList.Contains(u.KeyId);

            ModelPredicate = u => u.KeyId == CurrentModel.KeyId;
        }

    }
}
