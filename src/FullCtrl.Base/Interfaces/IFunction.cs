using System.Threading.Tasks;

namespace FullCtrl.Base
{
    public interface IFunction
    {
        Task<IFunctionResult> Execute(IFunctionArguments arguments);
    }
}
