using System;
using UnityEngine;
#region
//作者:Saber
#endregion
namespace Saber.Luban
{

    [AttributeUsage(AttributeTargets.Class)]
    public class AddressResourceAttribute : Attribute
    {
        public string runtimePath;
        public string editorPath;

    }
}
