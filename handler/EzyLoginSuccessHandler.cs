﻿using System;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.constant;

namespace com.tvd12.ezyfoxserver.client.handler
{
	public class EzyLoginSuccessHandler : EzyAbstractDataHandler
	{
		public override void handle(EzyArray data)
		{
			EzyArray joinedApps = data.get<EzyArray>(4);
			EzyData responseData = data.get<EzyData>(5);
			EzyUser user = newUser(data);
			EzyZone zone = newZone(data);
			((EzyMeAware)client).setMe(user);
			((EzyZoneAware)client).setZone(zone);
            handleResponseData(responseData);
            Boolean allowReconnect = allowReconnection();
            Int32 appCount = joinedApps.size();
            Boolean shouldConnect = allowReconnect && appCount > 0;
            if (shouldConnect)
            {
                handleResponseAppDatas(joinedApps);
                handleReconnectSuccess(responseData);
            }
            else 
            {
                handleLoginSuccess(responseData);
            }	
		}

        protected virtual bool allowReconnection() {
            return false;
        }

		protected virtual void handleResponseData(EzyData responseData)
		{
		}

		protected virtual void handleResponseAppDatas(EzyArray appDatas)
		{
			EzyDataHandler appAccessHandler =
					handlerManager.getDataHandler(EzyCommand.APP_ACCESS);
			for (int i = 0; i < appDatas.size(); i++)
			{
				EzyArray appData = appDatas.get<EzyArray>(i);
				EzyArray accessAppData = newAccessAppData(appData);
				appAccessHandler.handle(accessAppData);
			}
		}

        protected virtual EzyArray newAccessAppData(EzyArray appData)
		{
			return appData;
		}

        protected virtual EzyUser newUser(EzyArray data)
		{
			long userId = data.get<long>(2);
			String username = data.get<String>(3);
			EzySimpleUser user = new EzySimpleUser(userId, username);
			return user;
		}

        protected virtual EzyZone newZone(EzyArray data)
		{
			int zoneId = data.get<int>(0);
			String zoneName = data.get<String>(1);
			EzySimpleZone zone = new EzySimpleZone(client, zoneId, zoneName);
			return zone;
		}

		protected virtual void handleLoginSuccess(EzyData responseData)
		{
		}

		protected virtual void handleReconnectSuccess(EzyData responseData)
		{
            handleLoginSuccess(responseData);
		}
	}
}

