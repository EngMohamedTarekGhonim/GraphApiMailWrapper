using MailService;
using MailService.Dto;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GraphApiMailWrapper
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                GraphApiClient client = new GraphApiClient();
                Credentials credentials = new Credentials()
                {
                    TenantId = "eef8dc0f-4a8c-49a3-8291-23c88900a12a",
                    ClientId = "7615fd38-2b0e-4bf6-881d-54c1e742400d",
                    ClientSecretValue = "DVm8Q~PPpTU._RuuAQd~4.w3qUG029CQ2LxapciI",
                    Email = "api@securedsmartsystems.onmicrosoft.com",
                    Password = "3SEgypt@135" 
                    //Email = "mohamad.foda@3segypt.com",
                    //Password = "Fod@@P@ssw0rd@246"
                };
                client.Connect(credentials);
                var count = client.GetMessageCount();
                List<string> list = client.GetMessageUids();
                List<MessageHeader> messlist = new List<MessageHeader>();

                foreach (string id in list)
                {
                  var t =  client.GetMessageHeaders(id);
                    messlist.Add(t);
                }
              //  client.Delete("AAMkADY2NDIwNjZjLTgzZjgtNDkwMy1hOWUwLTZlMjkyNzEyM2E0NABGAAAAAACGQJNCX-g1RZjaznEOMBqLBwBIj-If1wBcT5Z9ZHIaxko_AAAAAAETAABIj-If1wBcT5Z9ZHIaxko_AAAHhtaoAAA=");

                //var x = wrapper.GetMAilBox();
                //foreach (var item in x.value) 
                //{
                //Console.WriteLine(item.subject);
                //Console.WriteLine(item.sender);
                //Console.WriteLine(item.sentDateTime);
                //    if(item.hasAttachments)
                //    {
                //        foreach(var resp in item.AttachmentsResponce.value)
                //        {
                //            Console.WriteLine(resp.name);
                //            Console.WriteLine(resp.contentType);
                //            //Console.WriteLine(resp.contentBytes);
                //            Console.WriteLine("-----------------------");
                //        }
                //    }
                //    Console.WriteLine("]]]]]]]]]]]]]]]]]]]]]]]]]]]]]");
                //}
                await Task.Delay(1000, stoppingToken);
            }
        }
        //protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        //{
        //    while (!stoppingToken.IsCancellationRequested)
        //    {
        //        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

        //        Wrapper wrapper = new Wrapper();
        //        wrapper.TenantId = "eef8dc0f-4a8c-49a3-8291-23c88900a12a";
        //        wrapper.ClientId = "7615fd38-2b0e-4bf6-881d-54c1e742400d";
        //        wrapper.ClientSecretValue = "DVm8Q~PPpTU._RuuAQd~4.w3qUG029CQ2LxapciI";
        //        wrapper.Email = "api@securedsmartsystems.onmicrosoft.com";
        //        wrapper.Password = "3SEgypt@135";
        //        wrapper.Email = "mohamad.foda@3segypt.com";
        //        wrapper.Password = "Fod@@P@ssw0rd@246";
        //        var x = wrapper.GetMessageUids();
        //        foreach (var item in x.value)
        //        {
        //            Console.WriteLine(item.subject);
        //            Console.WriteLine(item.sender);
        //            Console.WriteLine(item.sentDateTime);
        //            if (item.hasAttachments)
        //            {
        //                foreach (var resp in item.AttachmentsResponce.value)
        //                {
        //                    Console.WriteLine(resp.name);
        //                    Console.WriteLine(resp.contentType);
        //                    //Console.WriteLine(resp.contentBytes);
        //                    Console.WriteLine("-----------------------");
        //                }
        //            }
        //            Console.WriteLine("]]]]]]]]]]]]]]]]]]]]]]]]]]]]]");
        //        }
        //        await Task.Delay(1000, stoppingToken);
        //    }
        //}
    }
}
