using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookFast.Web.Infrastructure.Authentication.Customer
{
    internal class PolicyConfigurationManager : IConfigurationManager<OpenIdConnectConfiguration>
    {
        private const string PolicyParameter = "p";

        private readonly Dictionary<string, IConfigurationManager<OpenIdConnectConfiguration>> managers =
            new Dictionary<string, IConfigurationManager<OpenIdConnectConfiguration>>();

        public PolicyConfigurationManager(string authority, IEnumerable<string> policies)
        {
            foreach (var policy in policies)
            {
                var metadataAddress = $"{authority}/.well-known/openid-configuration?{PolicyParameter}={policy}";
                managers.Add(policy.ToLowerInvariant(), new ConfigurationManager<OpenIdConnectConfiguration>(metadataAddress, new OpenIdConnectConfigurationRetriever()));
            }
        }

        public async Task<OpenIdConnectConfiguration> GetConfigurationAsync(CancellationToken cancel)
        {
            OpenIdConnectConfiguration mergedConfiguration = null;
            foreach (var manager in managers)
            {
                var configuration = await manager.Value.GetConfigurationAsync(cancel);
                if (mergedConfiguration == null)
                    mergedConfiguration = Clone(configuration);
                else
                    MergeConfig(mergedConfiguration, configuration);
            }

            return mergedConfiguration;
        }

        public Task<OpenIdConnectConfiguration> GetConfigurationByPolicyAsync(CancellationToken cancel, string policy)
        {
            if (string.IsNullOrEmpty(policy))
                throw new ArgumentNullException(nameof(policy));

            var policyKey = policy.ToLowerInvariant();
            if (managers.ContainsKey(policyKey))
                return managers[policyKey].GetConfigurationAsync(cancel);

            throw new InvalidOperationException($"Invalid policy: {policy}");
        }

        public void RequestRefresh()
        {
            foreach (var manager in managers)
            {
                manager.Value.RequestRefresh();
            }
        }

        private static OpenIdConnectConfiguration Clone(OpenIdConnectConfiguration configuration)
        {
            var signingKeys = new List<SecurityKey>(configuration.SigningKeys);
            configuration.SigningKeys.Clear();

            var keySet = configuration.JsonWebKeySet;
            configuration.JsonWebKeySet = null;

            var json = OpenIdConnectConfiguration.Write(configuration);
            var clone = OpenIdConnectConfiguration.Create(json);

            foreach (var key in signingKeys)
            {
                configuration.SigningKeys.Add(key);
                clone.SigningKeys.Add(key);
            }

            configuration.JsonWebKeySet = keySet;
            clone.JsonWebKeySet = keySet;

            return clone;
        }

        private static void MergeConfig(OpenIdConnectConfiguration result, OpenIdConnectConfiguration source)
        {
            foreach (var alg in source.IdTokenSigningAlgValuesSupported)
            {
                if (!result.IdTokenSigningAlgValuesSupported.Contains(alg))
                {
                    result.IdTokenSigningAlgValuesSupported.Add(alg);
                }
            }

            foreach (var type in source.ResponseTypesSupported)
            {
                if (!result.ResponseTypesSupported.Contains(type))
                {
                    result.ResponseTypesSupported.Add(type);
                }
            }

            foreach (var type in source.SubjectTypesSupported)
            {
                if (!result.ResponseTypesSupported.Contains(type))
                {
                    result.SubjectTypesSupported.Add(type);
                }
            }

            foreach (var key in source.SigningKeys)
            {
                if (result.SigningKeys.All(k => k.KeyId != key.KeyId))
                {
                    result.SigningKeys.Add(key);
                }
            }
        }
    }
}
