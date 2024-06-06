using App.Settings;
using App.Settings.Actions;
using App.Settings.Actions.Rules;
using App.Settings.Actions.Types;
using App.Settings.ContractMap;
using App.Settings.ContractValidations;
using App.Settings.Strategies;
using App.Types;

namespace Test.Settings.Actions.Rules
{
    public class ActionSettingTypeHttpRequestRuleTest
    {
        public ActionSettingTypeHttpRequestRuleTest() 
        {
            new ApplicationSetting();
        }

        [Fact]
        public void WhenActionSettingTypeHttpRequestHasEmptyOrNullUri_ShouldReturnError()
        {
            // arrange
            var setting = new ActionSetting { Type = ActionType.HttpRequest, Uri = "" };
            var rule = new ActionSettingTypeHttpRequestRule();

            // act
            var result = rule.Do(setting);

            // assert
            var constains = result.Errors.Contains("Action.Uri is required");
            Assert.True(constains);
        }

        [Fact]
        public void WhenActionSettingTypeHttpRequestHasInvalidUri_ShouldReturnError()
        {
            // arrange
            var setting = new ActionSetting { Type = ActionType.HttpRequest, Uri = "test@com" };
            var rule = new ActionSettingTypeHttpRequestRule();

            // act
            var result = rule.Do(setting);

            // assert
            var constains = result.Errors.Contains("Action.Uri should a valid uri");
            Assert.True(constains);
        }

        [Fact]
        public void WhenActionSettingTypeHttpRequestHasValidUri_ShouldReturnSuccess()
        {
            // arrange
            var setting = new ActionSetting { Type = ActionType.HttpRequest, Uri = "http://google.com" };
            var rule = new ActionSettingTypeHttpRequestRule();

            // act
            var result = rule.Do(setting);

            // assert
            var constains = result.Errors.Contains("Action.Uri should a valid uri");
            Assert.False(constains);
        }


        [Fact]
        public void WhenActionSettingTypeHttpRequestHasEmptyMethod_ShouldReturnError()
        {
            // arrange
            var setting = new ActionSetting { Method = HttpMethodType.NONE };
            var rule = new ActionSettingTypeHttpRequestRule();

            // act
            var result = rule.Do(setting);

            // assert
            var constains = result.Errors.Contains("Action.Method is required");
            Assert.True(constains);
        }

        [Fact]
        public void WhenActionSettingTypeHttpRequestHasResponseContractMapIdNotConfigured_ShouldReturnError()
        {
            // arrange
            var setting = new ActionSetting { ReponseContractMapId = "not_configured" };
            var rule = new ActionSettingTypeHttpRequestRule();

            // act
            var result = rule.Do(setting);

            // assert
            var constains = result.Errors.Contains("Action.ReponseContractMapId not_configured should configured before use");
            Assert.True(constains);
        }

        [Fact]
        public void WhenActionSettingTypeHttpRequestHasResponseContractMapIdIsConfigured_ShouldReturnSuccess()
        {
            // arrange
            ApplicationSetting.Current.ContractMaps = new List<ContractMapSetting> { new ContractMapSetting { Id = "contract_configured" } };
            var setting = new ActionSetting { ReponseContractMapId = "contract_configured" };
            var rule = new ActionSettingTypeHttpRequestRule();

            // act
            var result = rule.Do(setting);

            // assert
            var constains = result.Errors.Contains("Action.ReponseContractMapId contract_configured should configured before use");
            Assert.False(constains);
        }

        [Fact]
        public void WhenActionSettingTypeHttpRequestHasRequestContractValidationIdNotConfigured_ShouldReturnError()
        {
            // arrange
            var setting = new ActionSetting { RequestContractValidationId = "not_configured" };
            var rule = new ActionSettingTypeHttpRequestRule();

            // act
            var result = rule.Do(setting);

            // assert
            var constains = result.Errors.Contains("Action.RequestContractValidationId not_configured should configured before use");
            Assert.True(constains);
        }

        [Fact]
        public void WhenActionSettingTypeHttpRequestHasRequestHasRequestContractValidationIdIsConfigured_ShouldReturnSuccess()
        {
            // arrange
            ApplicationSetting.Current.ContractValidations = new List<ContractValidation> { new ContractValidation { Id = "contract_configured" } };
            var setting = new ActionSetting { RequestContractValidationId = "contract_configured" };
            var rule = new ActionSettingTypeHttpRequestRule();

            // act
            var result = rule.Do(setting);

            // assert
            var constains = result.Errors.Contains("Action.RequestContractValidationId contract_configured should configured before use");
            Assert.False(constains);
        }

        [Theory]
        [InlineData(HttpMethodType.GET)]
        [InlineData(HttpMethodType.DELETE)]
        public void WhenActionSettingTypeHttpRequestHasRequestContractValidationIdCofiguredInvalidMethod_ShouldReturnError(HttpMethodType methodType)
        {
            // arrange
            var setting = new ActionSetting { RequestContractValidationId = "configured", Method = methodType };
            var rule = new ActionSettingTypeHttpRequestRule();

            // act
            var result = rule.Do(setting);

            // assert
            var constains = result.Errors.Contains("Action.RequestContractValidationId only used in post, put and patch methods");
            Assert.True(constains);
        }


        [Fact]
        public void WhenActionSettingTypeHttpRequestHasRequestStrategyCacheIdNotConfigured_ShouldReturnError()
        {
            // arrange
            ApplicationSetting.Current.Strategies = new StrategiesSetting { Caches = new List<CacheSetting> { new CacheSetting { Id = "cache_configured" } } };
            var setting = new ActionSetting { StrategyCacheId = "not_configured" };
            var rule = new ActionSettingTypeHttpRequestRule();

            // act
            var result = rule.Do(setting);

            // assert
            var constains = result.Errors.Contains("Action.StrategyCacheId not_configured should configured before use");
            Assert.True(constains);
        }

        [Fact]
        public void WhenActionSettingTypeHttpRequestHasRequestHasRequestStrategyCacheIdIsConfigured_ShouldReturnSuccess()
        {
            // arrange
            ApplicationSetting.Current.Strategies = new StrategiesSetting { Caches = new List<CacheSetting> { new CacheSetting { Id = "cache_configured" } } };
            var setting = new ActionSetting { StrategyCacheId = "cache_configured" };
            var rule = new ActionSettingTypeHttpRequestRule();

            // act
            var result = rule.Do(setting);

            // assert
            var constains = result.Errors.Contains("Action.StrategyCacheId cache_configured should configured before use");
            Assert.False(constains);
        }

        [Theory]
        [InlineData(HttpMethodType.POST)]
        [InlineData(HttpMethodType.PATCH)]
        [InlineData(HttpMethodType.PUT)]
        [InlineData(HttpMethodType.DELETE)]
        public void WhenActionSettingTypeHttpRequestGetAndHasStrategyCacheIdConfigured_ShouldReturnError(HttpMethodType methodType)
        {
            // arrange
            var setting = new ActionSetting { StrategyCacheId = "configured", Method = methodType };
            var rule = new ActionSettingTypeHttpRequestRule();

            // act
            var result = rule.Do(setting);

            // assert
            var constains = result.Errors.Contains("Action.StrategyCacheId only used in get methods");
            Assert.True(constains);
        }



        [Fact]
        public void WhenActionSettingTypeHttpRequestHasRequestStrategyHttpIdempotencyIdNotConfigured_ShouldReturnError()
        {
            // arrange
            var setting = new ActionSetting { StrategyHttpIdempotencyId = "not_configured" };
            var rule = new ActionSettingTypeHttpRequestRule();

            // act
            var result = rule.Do(setting);

            // assert
            var constains = result.Errors.Contains("Action.StrategyHttpIdempotencyId not_configured should configured before use");
            Assert.True(constains);
        }


        [Fact]
        public void WhenActionSettingTypeHttpRequestHasRequestHasRequestStrategyHttpIdempotencyIdIsConfigured_ShouldReturnSuccess()
        {
            // arrange
            ApplicationSetting.Current.
                Strategies = new StrategiesSetting
                {
                    HttpIdempotencies = new List<HttpIdempotencySetting>
                    {
                        new HttpIdempotencySetting { Id = "cache_configured" }
                    }
                };

            var setting = new ActionSetting { StrategyHttpIdempotencyId = "cache_configured" };
            var rule = new ActionSettingTypeHttpRequestRule();

            // act
            var result = rule.Do(setting);

            // assert
            var constains = result.Errors.Contains("Action.StrategyHttpIdempotencyId cache_configured should configured before use");
            Assert.False(constains);
        }

        [Theory]
        [InlineData(HttpMethodType.GET)]
        public void WhenActionSettingTypeHttpRequestGetAndHasStrategyHttpIdempotencyIdConfigured_ShouldReturnError(HttpMethodType methodType)
        {
            // arrange
            ApplicationSetting.Current.
               Strategies = new StrategiesSetting
               {
                   HttpIdempotencies = new List<HttpIdempotencySetting>
                   {
                        new HttpIdempotencySetting { Id = "configured" }
                   }
               };
            var setting = new ActionSetting { StrategyHttpIdempotencyId = "configured", Method = methodType };
            var rule = new ActionSettingTypeHttpRequestRule();

            // act
            var result = rule.Do(setting);

            // assert
            var constains = result.Errors.Contains("Action.StrategyHttpIdempotencyId only used in post, put, patch and delete methods");
            Assert.True(constains);
        }
    }
}
