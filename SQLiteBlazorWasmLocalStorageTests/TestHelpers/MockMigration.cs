using SQLiteBlazorWasmLocalStorage;

namespace SQLiteBlazorWasmLocalStorageTests.TestHelpers
{
    public class MockMigration : IMigration
    {
        public bool UseMigration() => false;
    }
}
