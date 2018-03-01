using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using System.Collections.Generic;
using System.Fabric;
using System.Globalization;

namespace BookFast.ServiceFabric.AppInsights
{
    internal class FabricTelemetryInitializer : ITelemetryInitializer
    {
        private readonly Dictionary<string, string> contextCollection;

        public FabricTelemetryInitializer(ServiceContext context)
        {
            contextCollection = GetContextDictionaryFromServiceContext(context);
        }

        public void Initialize(ITelemetry telemetry)
        {
            foreach (var field in contextCollection)
            {
                if (!telemetry.Context.Properties.ContainsKey(field.Key))
                {
                    telemetry.Context.Properties.Add(field.Key, field.Value);
                }
            }

            if (string.IsNullOrEmpty(telemetry.Context.Cloud.RoleName) && contextCollection.ContainsKey(KnownContextFieldNames.ServiceName))
            {
                telemetry.Context.Cloud.RoleName = contextCollection[KnownContextFieldNames.ServiceName];
            }

            if (string.IsNullOrEmpty(telemetry.Context.Cloud.RoleInstance))
            {
                if (contextCollection.ContainsKey(KnownContextFieldNames.InstanceId))
                {
                    telemetry.Context.Cloud.RoleInstance = contextCollection[KnownContextFieldNames.InstanceId];
                }
                else if (this.contextCollection.ContainsKey(KnownContextFieldNames.ReplicaId))
                {
                    telemetry.Context.Cloud.RoleInstance = contextCollection[KnownContextFieldNames.ReplicaId];
                }
            }
        }

        private static Dictionary<string, string> GetContextDictionaryFromServiceContext(ServiceContext context)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            if (context != null)
            {
                result.Add(KnownContextFieldNames.ServiceName, context.ServiceName.ToString());
                result.Add(KnownContextFieldNames.ServiceTypeName, context.ServiceTypeName);
                result.Add(KnownContextFieldNames.PartitionId, context.PartitionId.ToString());
                result.Add(KnownContextFieldNames.ApplicationName, context.CodePackageActivationContext.ApplicationName);
                result.Add(KnownContextFieldNames.ApplicationTypeName, context.CodePackageActivationContext.ApplicationTypeName);
                result.Add(KnownContextFieldNames.NodeName, context.NodeContext.NodeName);

                if (context is StatelessServiceContext)
                {
                    result.Add(KnownContextFieldNames.InstanceId, context.ReplicaOrInstanceId.ToString(CultureInfo.InvariantCulture));
                }

                if (context is StatefulServiceContext)
                {
                    result.Add(KnownContextFieldNames.ReplicaId, context.ReplicaOrInstanceId.ToString(CultureInfo.InvariantCulture));
                }
            }

            return result;
        }

        private class KnownContextFieldNames
        {
            public const string ServiceName = "ServiceFabric.ServiceName";
            public const string ServiceTypeName = "ServiceFabric.ServiceTypeName";
            public const string PartitionId = "ServiceFabric.PartitionId";
            public const string ApplicationName = "ServiceFabric.ApplicationName";
            public const string ApplicationTypeName = "ServiceFabric.ApplicationTypeName";
            public const string NodeName = "ServiceFabric.NodeName";
            public const string InstanceId = "ServiceFabric.InstanceId";
            public const string ReplicaId = "ServiceFabric.ReplicaId";
        }
    }
}
