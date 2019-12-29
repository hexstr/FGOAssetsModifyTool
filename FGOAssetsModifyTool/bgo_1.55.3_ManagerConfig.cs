using System;

// Token: 0x0200069E RID: 1694
public class ManagerConfig
{
	// Token: 0x04002DED RID: 11757
	public static readonly string AppVer = "1.55.3";

	// Token: 0x04002DEE RID: 11758
	public static readonly string AppBuildDate = "20170307_00:00";

	// Token: 0x04002DEF RID: 11759
	public static readonly string SaveDataVer = "Fgo_20150511_1";

	// Token: 0x04002DF0 RID: 11760
	public static readonly string MasterDataCacheVer = "Fgo_20180629_1";

	// Token: 0x04002DF1 RID: 11761
	public static readonly float TIMEOUT = 30f;

	// Token: 0x04002DF2 RID: 11762
	public static readonly float GAME_DATA_TIMEOUT = 30f;

	// Token: 0x04002DF3 RID: 11763
	public static readonly float WORK_TIME = 1f;

	// Token: 0x04002DF4 RID: 11764
	public static readonly float SLOW_LOAD_WAIT_TIME = 2f;

	// Token: 0x04002DF5 RID: 11765
	public static readonly float SLOW_CONNECT_WAIT_TIME = 2f;

	// Token: 0x04002DF6 RID: 11766
	public static readonly long ACCOUNTING_INITIALIZE_TIMEOUT = 30L;

	// Token: 0x04002DF7 RID: 11767
	public static readonly long SERVER_TIME_OVER_LIMIT = 3600L;

	// Token: 0x04002DF8 RID: 11768
	public static readonly float DOWNLOAD_RETRY_DELAY_TIME = 10f;

	// Token: 0x04002DF9 RID: 11769
	public static readonly int RETRY_COUNT = 3;

	// Token: 0x04002DFA RID: 11770
	public static readonly int WIDTH = 1024;

	// Token: 0x04002DFB RID: 11771
	public static readonly int HEIGHT = 576;

	// Token: 0x04002DFC RID: 11772
	public static readonly long LIMIT_FREE_SIZE = 52428800L;

	// Token: 0x04002DFD RID: 11773
	public static readonly float MINIMUM_ENABLE_ALPHA = 0.005f;

	// Token: 0x04002DFE RID: 11774
	public static readonly string AndroidProjectID = "1";

	// Token: 0x04002DFF RID: 11775
	public static readonly string AndroidPackageName = "com.aniplex.fategrandorder";

	// Token: 0x04002E00 RID: 11776
	public static readonly string iOSApplicationID = "1";

	// Token: 0x04002E01 RID: 11777
	public static readonly string PlatformName = "Android";

	// Token: 0x04002E02 RID: 11778
	public static readonly bool ReleaseNetworkSecurity = true;

	// Token: 0x04002E03 RID: 11779
	public static readonly string ReleaseGameServerAddress = "game.fatedomaindummy.jp";

	// Token: 0x04002E04 RID: 11780
	public static readonly string ReleaseDataServerAddress = "http://data.fatedomaindummy.jp/AssetStorages";

	// Token: 0x04002E05 RID: 11781
	public static readonly string ReleaseWebServerAddress = "webview.fatedomaindummy.jp";

	// Token: 0x04002E06 RID: 11782
	public static readonly bool ReviewNetworkSecurity = false;

	// Token: 0x04002E07 RID: 11783
	public static readonly string ReviewGameServerAddress = "fgoios-fg8gf.fategrandorder.jp";

	// Token: 0x04002E08 RID: 11784
	public static readonly string ReviewDataServerAddress = "https://fgoios-fg8gf-asset.fategrandorder.jp/AssetStorages";

	// Token: 0x04002E09 RID: 11785
	public static readonly string ReviewWebServerAddress = "fgo.biligame.com";

	// Token: 0x04002E0A RID: 11786
	public static readonly bool StagingNetworkSecurity = false;

	// Token: 0x04002E0B RID: 11787
	public static readonly string StagingGameServerAddress = "staging.fatedomaindummy.net";

	// Token: 0x04002E0C RID: 11788
	public static readonly string StagingDataServerAddress = "http://data.fatedomaindummy.com/Assets";

	// Token: 0x04002E0D RID: 11789
	public static readonly string StagingWebServerAddress = "fgo.biligame.com";

	// Token: 0x04002E0E RID: 11790
	public static readonly bool GameCloneNetworkSecurity = false;

	// Token: 0x04002E0F RID: 11791
	public static readonly string GameCloneGameServerAddress = "clone.fatedomaindummy.net";

	// Token: 0x04002E10 RID: 11792
	public static readonly string GameCloneDataServerAddress = "http://data.fatedomaindummy.com/Assets";

	// Token: 0x04002E11 RID: 11793
	public static readonly string GameCloneWebServerAddress = "fgo.biligame.com";

	// Token: 0x04002E12 RID: 11794
	public static readonly bool ReleaseCloneNetworkSecurity = false;

	// Token: 0x04002E13 RID: 11795
	public static readonly string ReleaseCloneGameServerAddress = "rclone.fatedomaindummy.net";

	// Token: 0x04002E14 RID: 11796
	public static readonly string ReleaseCloneDataServerAddress = "http://data.fatedomaindummy.com/Assets";

	// Token: 0x04002E15 RID: 11797
	public static readonly string ReleaseCloneWebServerAddress = "fgo.biligame.com";

	// Token: 0x04002E16 RID: 11798
	public static readonly bool DebugNetworkSecurity = false;

	// Token: 0x04002E17 RID: 11799
	public static readonly string DebugGameServerAddress = "debug1.fatedomaindummy.net";

	// Token: 0x04002E18 RID: 11800
	public static readonly string DebugDataServerAddress = "http://data.fatedomaindummy.com/Assets";

	// Token: 0x04002E19 RID: 11801
	public static readonly string DebugWebServerAddress = "webview.fatedomaindummy.com";

	// Token: 0x04002E1A RID: 11802
	public static readonly bool EventDevNetworkSecurity = false;

	// Token: 0x04002E1B RID: 11803
	public static readonly string EventDevGameServerAddress = "event.fatedomaindummy.net";

	// Token: 0x04002E1C RID: 11804
	public static readonly string EventDevDataServerAddress = "http://data.fatedomaindummy.com/Assets";

	// Token: 0x04002E1D RID: 11805
	public static readonly string EventDevWebServerAddress = "event.fatedomaindummy.net";

	// Token: 0x04002E1E RID: 11806
	public static readonly bool DevelopNetworkSecurity = false;

	// Token: 0x04002E1F RID: 11807
	public static string DevelopGameServerAddress = string.Empty;

	// Token: 0x04002E20 RID: 11808
	public static string DevelopDataServerAddress = string.Empty;

	// Token: 0x04002E21 RID: 11809
	public static string DevelopCDNAddress = string.Empty;

	// Token: 0x04002E22 RID: 11810
	public static readonly string DevelopWebServerAddress = "fgo.biligame.com";

	// Token: 0x04002E23 RID: 11811
	public static string DevelopGonggaoAddress = string.Empty;

	// Token: 0x04002E24 RID: 11812
	public static string AfterDomainNameUrlCDN = string.Empty;

	// Token: 0x04002E25 RID: 11813
	public static readonly bool QaNetworkSecurity = false;

	// Token: 0x04002E26 RID: 11814
	public static readonly string QaGameServerAddress = "qa{0}-fg8gf.fategrandorder.jp";

	// Token: 0x04002E27 RID: 11815
	public static readonly string QaDataServerAddress = "http://data.fatedomaindummy.com/Assets";

	// Token: 0x04002E28 RID: 11816
	public static readonly string QaWebServerAddress = "fgo.biligame.com";

	// Token: 0x04002E29 RID: 11817
	public static readonly bool DevNetworkSecurity = false;

	// Token: 0x04002E2A RID: 11818
	public static readonly string Dev1GameServerAddress = "dev1.fatedomaindummy.net";

	// Token: 0x04002E2B RID: 11819
	public static readonly string Dev2GameServerAddress = "dev2.fatedomaindummy.net";

	// Token: 0x04002E2C RID: 11820
	public static readonly string Dev3GameServerAddress = "dev3.fatedomaindummy.net";

	// Token: 0x04002E2D RID: 11821
	public static readonly string Dev4GameServerAddress = "dev4.fatedomaindummy.net";

	// Token: 0x04002E2E RID: 11822
	public static readonly bool PlanNetworkSecurity = false;

	// Token: 0x04002E2F RID: 11823
	public static readonly string PlanGameServerAddress = "plan{0}-fg8gf.fategrandorder.jp";

	// Token: 0x04002E30 RID: 11824
	public static readonly string PlanDataServerAddress = "http://data.fatedomaindummy.com/Assets";

	// Token: 0x04002E31 RID: 11825
	public static readonly bool LimitNetworkSecurity = false;

	// Token: 0x04002E32 RID: 11826
	public static readonly string LimitGameServerAddress = "lmt{0}-fg8gf.fategrandorder.jp";

	// Token: 0x04002E33 RID: 11827
	public static readonly string LimitDataServerAddress = "https://fgodev-fg8gf-asset.fategrandorder.jp/Assets";

	// Token: 0x04002E34 RID: 11828
	public static readonly bool ExternalNetworkSecurity = false;

	// Token: 0x04002E35 RID: 11829
	public static readonly string ExternalGameServerAddress = "external{0}-fg8gf.fategrandorder.jp";

	// Token: 0x04002E36 RID: 11830
	public static readonly string ExternalDataServerAddress = "https://fgodev-fg8gf-asset.fategrandorder.jp/Assets";

	// Token: 0x04002E37 RID: 11831
	public static readonly bool PlayNetworkSecurity = false;

	// Token: 0x04002E38 RID: 11832
	public static readonly string PlayGameServerAddress = "play{0}-fg8gf.fategrandorder.jp";

	// Token: 0x04002E39 RID: 11833
	public static readonly string PlayDataServerAddress = "https://fgodev-fg8gf-asset.fategrandorder.jp/Assets";

	// Token: 0x04002E3A RID: 11834
	public static readonly bool VersionUpNetworkSecurity = false;

	// Token: 0x04002E3B RID: 11835
	public static readonly string VersionUp1GameServerAddress = "verup01-fg8gf.fategrandorder.jp";

	// Token: 0x04002E3C RID: 11836
	public static readonly string VersionUp1DataServerAddress = "https://fgodev-fg8gf-asset.fategrandorder.jp/Assets";

	// Token: 0x04002E3D RID: 11837
	public static readonly string VersionUpWebServerAddress = "webview.fgo.akamai.delightworks.tech";

	// Token: 0x04002E3E RID: 11838
	public static readonly bool StressNetworkSecurity = true;

	// Token: 0x04002E3F RID: 11839
	public static readonly string StressGameServerAddress = "stress.fatedomaindummy.net";

	// Token: 0x04002E40 RID: 11840
	public static readonly string StressDataServerAddress = "http://data.fatedomaindummy.com/Assets";

	// Token: 0x04002E41 RID: 11841
	public static readonly string DevDataServerAddress = "http://data.fatedomaindummy.com/Assets";

	// Token: 0x04002E42 RID: 11842
	public static readonly string DevWebServerAddress = "fgo.biligame.com";

	// Token: 0x04002E43 RID: 11843
	public static readonly string ScriptGameServerAddress = "file://C:/FGO/Temporary/AssetStorages/Mock";

	// Token: 0x04002E44 RID: 11844
	public static readonly string ScriptDataServerAddress = "file://C:/FGO/Temporary/AssetStorages";

	// Token: 0x04002E45 RID: 11845
	public static readonly string DevelopmentAuthCode = "aK8mTxBJCwZyxBjNJSKA5xCWL7zKtgZEQNiZmffXUbyQd5aLun";

	// Token: 0x04002E46 RID: 11846
	public static readonly bool IsTrueTargetPlatform = true;

	// Token: 0x04002E47 RID: 11847
	public static readonly bool UseClientNetworkParameterCheck = true;

	// Token: 0x04002E48 RID: 11848
	public static readonly bool IsNetworkMock = false;

	// Token: 0x04002E49 RID: 11849
	public static readonly string ServerDefaultType = "GAME_SERVER";

	// Token: 0x04002E4A RID: 11850
	public static readonly bool UseAppServer = ManagerConfig.IsTrueTargetPlatform;

	// Token: 0x04002E4B RID: 11851
	public static readonly bool UseDebugCommand = false;

	// Token: 0x04002E4C RID: 11852
	public static readonly bool UseStandaloneAsset = true;

	// Token: 0x04002E4D RID: 11853
	public static readonly bool UseMock = false;

	// Token: 0x04002E4E RID: 11854
	public static readonly bool UseWebViewAuthoring = false;

	// Token: 0x04002E4F RID: 11855
	public static string EffectList = "[{\"id\":14300,\"effectNameList\":[\"eff/_dust_thick\"]}]";
}