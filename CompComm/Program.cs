﻿using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace CompComm {
  /// <summary>Main program class.</summary>
  public class Program {
    /// <summary>Default port to use if none passed in.</summary>
    private const int DEFAULT_PORT = 1996;

    /// <summary>Main program method.</summary>
    /// <param name="args">Additional arguments.
    /// <code>--port</code> = port number to listen to.</param>
    public static void Main(string[] args) {
      var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("hosting.json", optional: true)
        .Build();

      int idx = Array.IndexOf(args, "--port");
      int portNum = idx == -1? DEFAULT_PORT : int.Parse(args[idx + 1]);

      try {
        var host = new WebHostBuilder()
          .UseConfiguration(config)
          .UseKestrel(options =>
            options.Listen(IPAddress.Loopback, portNum))
          .UseStartup<Startup>()
          .Build();
        host.Run();

      } catch (System.Security.Cryptography.CryptographicException) {
        Console.WriteLine("Error: SSL certificate either could not be found or was invalid.");
      } catch (Exception e) {
        Console.WriteLine(e.Message);
      }
    }
  }
}
