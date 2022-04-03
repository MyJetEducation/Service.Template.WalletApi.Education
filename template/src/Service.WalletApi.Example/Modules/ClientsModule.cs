using Autofac;
using MyJetWallet.Sdk.Authorization.NoSql;
using MyJetWallet.Sdk.NoSql;
using Service.AssetsDictionary.Client;
using Service.ClientProfile.Client;
using Service.ClientWallets.Client;

namespace Service.WalletApi.Example.Modules
{
    public class ClientsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
			RegisterAuthServices(builder);
        }

		protected void RegisterAuthServices(ContainerBuilder builder)
        {
            var authNoSql = builder.CreateNoSqlClient(() => Program.Settings.AuthMyNoSqlReaderHostPort);
            builder.RegisterMyNoSqlReader<ShortRootSessionNoSqlEntity>(authNoSql, ShortRootSessionNoSqlEntity.TableName);
        }
    }
}
