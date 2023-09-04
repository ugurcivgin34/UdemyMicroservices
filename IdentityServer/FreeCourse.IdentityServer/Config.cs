// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;

namespace FreeCourse.IdentityServer
{
    public static class Config
    {
        // API kaynaklarını tanımlayan özellik
        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
        {
        // "resource_catalog" isimli API kaynağını tanımlama. Bu kaynak için "catalog_fullpermission" adında bir yetki (scope) belirlenmiş.
        new ApiResource("resource_catalog"){Scopes={"catalog_fullpermission"}},
        // "photo_stock_catalog" isimli API kaynağını tanımlama. Bu kaynak için "photo_stock_fullpermission" adında bir yetki (scope) belirlenmiş.
        new ApiResource("photo_stock_catalog"){Scopes={"photo_stock_fullpermission"}},
        new ApiResource("resource_basket"){Scopes={"basket_fullpermission"}},
        new ApiResource("resource_discount"){Scopes={"discount_fullpermission"}},
        new ApiResource("resource_order"){Scopes={"order_fullpermission"}},
        // IdentityServer'ın yerel API'si için kaynak tanımlama
        new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
        };

        // Kimlik doğrulama işlemi sırasında talep edilebilecek özellikler
        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                   // Kullanıcının e-posta adresine erişim yetkisi
                   new IdentityResources.Email(),
                   // OpenID Connect protokolü için zorunlu özellik
                   new IdentityResources.OpenId(),
                   // Kullanıcının profil bilgilerine erişim yetkisi
                   new IdentityResources.Profile(),
                   // Kullanıcı rollerine erişim yetkisi. Kullanıcı talepleri arasında "role" bilgisini ekler.
                   new IdentityResource(){ Name="roles", DisplayName="Roles", Description="Kullanıcı rolleri", UserClaims=new []{ "role"} }
                   };

        // API yetkilerini (scopes) tanımlayan özellik
        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
            // "catalog_fullpermission" yetkisi, "Catalog API için full erişim" olarak tanımlanmış.
            new ApiScope("catalog_fullpermission","Catalog API için full erişim"),
            // "photo_stock_fullpermission" yetkisi, "Photo Stock API için full erişim" olarak tanımlanmış.
            new ApiScope("photo_stock_fullpermission","Photo Stock API için full erişim"),
            new ApiScope("basket_fullpermission","Basket API için full erişim"),
            new ApiScope("discount_fullpermission","Discount API için full erişim"),
            new ApiScope("order_fullpermission","Order API için full erişim"),
            // IdentityServer'ın yerel API'si için yetki tanımlama
            new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            };

        // Client'ların (istemcilerin) tanımlamaları
        public static IEnumerable<Client> Clients =>
            new Client[]
            {
            new Client
            {
                ClientName="Asp.Net Core MVC",
                ClientId="WebMvcClient",
                ClientSecrets= {new Secret("secret".Sha256())},
                AllowedGrantTypes= GrantTypes.ClientCredentials,
                AllowedScopes={ "catalog_fullpermission","photo_stock_fullpermission",IdentityServerConstants.LocalApi.ScopeName }
            },
            new Client
            {
                ClientName="Asp.Net Core MVC", // Client'ın adı
                ClientId="WebMvcClientForUser", // Client'ın ID'si
                AllowOfflineAccess=true, // Refresh token alabilmek için
                ClientSecrets= {new Secret("secret".Sha256())}, 
                AllowedGrantTypes= GrantTypes.ResourceOwnerPassword,
                AllowedScopes={"basket_fullpermission", // Basket API için full erişim
                    "discount_fullpermission", // Discount API için full erişim
                    "order_fullpermission",
                    IdentityServerConstants.StandardScopes.Email,  // Kullanıcının e-posta adresine erişim yetkisi
                    IdentityServerConstants.StandardScopes.OpenId, // OpenID Connect protokolü için zorunlu özellik
                    IdentityServerConstants.StandardScopes.Profile, // Kullanıcı profil bilgileri
                    IdentityServerConstants.StandardScopes.OfflineAccess, // Refresh token alabilmek için
                    IdentityServerConstants.LocalApi.ScopeName,"roles" }, // "roles" yetkisi, kullanıcı rollerine erişim için tanımlanmıştı.
                AccessTokenLifetime=1*60*60, 
                RefreshTokenExpiration=TokenExpiration.Absolute, // Refresh token'ın ömrü
                AbsoluteRefreshTokenLifetime= (int) (DateTime.Now.AddDays(60)- DateTime.Now).TotalSeconds, // Refresh token'ın ömrü
                RefreshTokenUsage= TokenUsage.ReUse // Refresh token'ın kullanımı
            }
            };
    }

}