using Lf.Slack.TransactionBalance.Infrastructure;
using Microsoft.Data.Sqlite;
using SQLite;

namespace Lf.Slack.Infrastructure.Setup;
public static class Database
{
    public static void Setup()
    {

        var _db = new SQLiteConnection("./infrastructure/lfslack.db");
        _db.CreateTable<PersistableTransactionBalance>();
        _db.CreateTable<PersistableRefund>();
    }
}