﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Net;
//using System.Diagnostics;
//using HtmlAgilityPack;
//using Remotus.Base;

//namespace Remotus.Core.Net
//{
//    public class FantasyPremierLeague2016Authenticator : IAuthenticator
//    {
//        private static string _URL_LOGIN = "https://fantasy.premierleague.com/accounts/login/";
//        private static string _URL_LOGIN_CONFIRM = "https://fantasy.premierleague.com/a/login?a={0}&state=success&r={1}&e=3600&s=read+write";
//        private static string _FIELD_USERNAME = "login";
//        private static string _FIELD_PASSWORD = "password";

//        private ILog _log;

//        public FantasyPremierLeague2016Authenticator(ILog log)
//        {
//            _log = log;
//        }


//        public CookieContainer Authenticate(string username, string password)
//        {
//            try
//            {
//                System.Net.HttpWebResponse resp;
//                var requester = new WebPageRequester(_log);
//                var cookies = new CookieContainer();

//                _log.Debug("GET Login");
//                //var html = requester.Get(_URL_LOGIN, ref cookies, out resp);
//                var url = "https://users.premierleague.com/accounts/login/";
//                var html = requester.MakeRequest(
//                    url: url,
//                    contenttype: null,
//                    method: "GET",
//                    data: null,
//                    refCookies: ref cookies,
//                    referer: null
//                );
//                DumpCookies(cookies);



//                var uri = new Uri("https://users.premierleague.com/");
//                var c = cookies.GetCookies(uri);
//                var csrfmiddlewaretoken = c["csrftoken"]?.Value;

//                if (string.IsNullOrWhiteSpace(csrfmiddlewaretoken))
//                {
//                    var htmlDoc = new HtmlDocument();
//                    htmlDoc.LoadHtml(html);
//                    var loginForm = htmlDoc.DocumentNode.SelectSingleNode("//form[@class='ism-form__login-box']");
//                    var csrfHiddenInput = loginForm?.SelectSingleNode("//input[@name='csrfmiddlewaretoken']");
//                    csrfmiddlewaretoken = csrfHiddenInput?.GetAttributeValue("value", null);
//                }

//                var parameters = "";
//                parameters += string.Format("{0}={1}&", "csrfmiddlewaretoken", csrfmiddlewaretoken);
//                parameters += string.Format("{0}={1}&", _FIELD_USERNAME, Uri.EscapeDataString(username));
//                parameters += string.Format("{0}={1}&", _FIELD_PASSWORD, Uri.EscapeDataString(password));
//                parameters += string.Format("{0}={1}&", "app", "plusers");
//                parameters += string.Format("{0}={1}&", "redirect_uri", "https://users.premierleague.com/");
//                parameters = parameters.Trim('&');
//                //var response = requester.Post(_URL_LOGIN, parameters, ref cookies, out resp);
//                url = "https://users.premierleague.com/accounts/login/";
//                _log.Debug("POST Login");
//                var response = requester.MakeRequest(
//                    url: url,
//                    contenttype: "application/x-www-form-urlencoded",
//                    method: "POST",
//                    data: parameters,
//                    refCookies: ref cookies,
//                    referer: "https://users.premierleague.com/"
//                );
//                DumpCookies(cookies);


//                _log.Debug("Begin copy of cookies");
//                // copy cookies between sub-domains
//                var uriSource = new Uri("https://users.premierleague.com/");
//                var uriTarget = new Uri("https://fantasy.premierleague.com/");
//                var sourceCookies = cookies.GetCookies(uriSource);


//                var sCookie = sourceCookies["sessionid"];
//                if (sCookie != null)
//                {
//                    cookies.SetCookies(uriTarget, sCookie.ToString());
//                }
//                sCookie = sourceCookies["pl_profile"];
//                if (sCookie != null)
//                {
//                    cookies.SetCookies(uriTarget, sCookie.ToString());
//                }
//                if (string.IsNullOrWhiteSpace(cookies.GetCookies(uriTarget)["csrftoken"]?.Value))
//                {
//                    sCookie = sourceCookies["csrftoken"];
//                    if (sCookie != null)
//                        cookies.SetCookies(uriTarget, sCookie.ToString());
//                }


//                uriTarget = new Uri("https://premierleague.com/");
//                sourceCookies = cookies.GetCookies(uriSource);
//                if (string.IsNullOrWhiteSpace(cookies.GetCookies(uriTarget)["sessionid"]?.Value))
//                {
//                    sCookie = sourceCookies["sessionid"];
//                    if (sCookie != null)
//                        cookies.SetCookies(uriTarget, sCookie.ToString());
//                }
//                if (string.IsNullOrWhiteSpace(cookies.GetCookies(uriTarget)["pl_profile"]?.Value))
//                {
//                    sCookie = sourceCookies["pl_profile"];
//                    if (sCookie != null)
//                        cookies.SetCookies(uriTarget, sCookie.ToString());
//                }
//                if (string.IsNullOrWhiteSpace(cookies.GetCookies(uriTarget)["csrftoken"]?.Value))
//                {
//                    sCookie = sourceCookies["csrftoken"];
//                    if (sCookie != null)
//                        cookies.SetCookies(uriTarget, sCookie.ToString());
//                }
//                _log.Debug("Cookies have been copied");
//                DumpCookies(cookies);


//                var testUri = new Uri("https://fantasy.premierleague.com/drf/transfers");
//                var testRequest = requester.MakeRequest(
//                    url: testUri.OriginalString,
//                    contenttype: null,
//                    method: "GET",
//                    data: null,
//                    refCookies: ref cookies,
//                    referer: null
//                );

//                uriTarget = new Uri("https://fantasy.premierleague.com/");
//                var targetCookies = cookies.GetCookies(uriTarget);
//                var sessionCookie = targetCookies["sessionid"];
//                if (sessionCookie == null || string.IsNullOrEmpty(sessionCookie.Value))
//                {
//                    throw new Exception("Could not authenticate");
//                }

//                return cookies;
//            }
//            catch (WebException e)
//            {
//                _log.Error("Authenticate - Error occurred", e);
//            }
//            catch (Exception e)
//            {
//                _log.Error("Authenticate - Error occurred", e);
//            }

//            return null;
//        }


//        public void DumpCookies(CookieContainer cookieContainer)
//        {
//            _log.Debug($"Cookies:");
//            DumpCookies(cookieContainer, new Uri("https://fantasy.premierleague.com/"));
//            DumpCookies(cookieContainer, new Uri("https://users.premierleague.com/"));
//            DumpCookies(cookieContainer, new Uri("https://premierleague.com/"));
//        }

//        public void DumpCookies(CookieContainer cookieContainer, Uri uri)
//        {
//            _log.Debug($"Cookies @{uri}:");

//            var cookies = cookieContainer.GetCookies(uri);
//            foreach (var cookie in cookies.OfType<Cookie>())
//            {
//                var msg = $"{cookie.Name}: {cookie.Value}";
//                _log.Debug(msg);
//            }
//        }



//        public class WebPageRequester
//        {
//            private const string _HEADERS_ACCEPT = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
//            private const string _HEADERS_ACCEPT_ENCODING = "gzip, deflate";
//            private const string _HEADERS_ACCEPT_LANGUAGE = "en-US,en;q=0.8,sv;q=0.6";
//            private const string _HEADERS_USER_AGENT = "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Safari/537.36"; // just a dummy user agent that looks like a real browser

//            private ILog _log;

//            public WebPageRequester(ILog log)
//            {
//                _log = log;
//            }


//            public string Post(string url, string data, ref CookieContainer refCookies)
//            {
//                return MakeRequest(url, "application/x-www-form-urlencoded", "POST", data, ref refCookies);
//            }

//            public string Get(string url, ref CookieContainer refCookies)
//            {
//                return MakeRequest(url, "text/html", "GET", null, ref refCookies);
//            }


//            #region Private Methods

//            public string MakeRequest(string url, string contenttype, string method, string data, ref CookieContainer refCookies, string referer = null)
//            {
//                // create new request
//                var req = (HttpWebRequest)System.Net.HttpWebRequest.Create(url);

//                // add headers as per browser
//                req.Accept = _HEADERS_ACCEPT;
//                req.UserAgent = _HEADERS_USER_AGENT;
//                req.Headers["Accept-Encoding"] = _HEADERS_ACCEPT_ENCODING;
//                req.Headers["Accept-Language"] = _HEADERS_ACCEPT_LANGUAGE;
//                req.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
//                req.Headers["Upgrade-Insecure-Requests"] = "1";
//                if (method == "POST")
//                {
//                    req.Referer = referer;
//                    req.Headers["Origin"] = referer?.Trim('/');
//                    req.Headers["Cache-Control"] = "max-age=0";
//                }

//                // Set method/content type
//                req.ContentType = contenttype;
//                req.Method = method;

//                // Add cookies
//                if (refCookies == null)
//                    refCookies = new CookieContainer();
//                req.CookieContainer = refCookies;

//                if (data != null)
//                {
//                    _log.Debug("WebPageRequester.MakeRequest - Sending data");

//                    byte[] bytes = System.Text.Encoding.ASCII.GetBytes(data);
//                    req.ContentLength = bytes.Length;

//                    using (System.IO.Stream os = req.GetRequestStream())
//                    {
//                        os.Write(bytes, 0, bytes.Length); //Push it out there
//                        os.Close();
//                    }
//                }

//                _log.Debug("WebPageRequester.MakeRequest - Reading response");

//                using (var resp = (HttpWebResponse)req.GetResponse())
//                {
//                    if (resp == null)
//                        _log.Error("WebPageRequester.MakeRequest - No response received");
//                    System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
//                    var strResponse = sr.ReadToEnd().Trim();
//                    // _logger.WriteInfoMessage("WebPageRequester.MakeRequest - Response: " + strResponse);
//                    _log.Debug("WebPageRequester.MakeRequest - Response Content Length: " + resp.ContentLength);
//                    _log.Debug("WebPageRequester.MakeRequest - Response Content Encoding: " + resp.ContentEncoding);
//                    _log.Debug("WebPageRequester.MakeRequest - Response Content Type: " + resp.ContentType);
//                    _log.Debug("WebPageRequester.MakeRequest - Response Character Set: " + resp.CharacterSet);
//                    return strResponse;
//                }
//            }

//            #endregion
//        }
//    }
//}
