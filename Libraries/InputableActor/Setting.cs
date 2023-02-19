using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InputableActor
{
    public interface ISettingLoadable
    {
        void LoadSetting<T>(T setting) { }
    }
}