using FCG.Application.UseCases.Library.GetMyLibrary;

namespace FCG.CommomTestsUtilities.Builders.Requests
{
    public class GetMyLibraryInputBuilder
    {
        public static GetMyLibraryRequest Build()
        {
            return new GetMyLibraryRequest();
        }
    }
}
