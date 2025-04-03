<a id="readme-top"></a>

[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![Unlicense License][license-shield]][license-url]



<!-- PROJECT LOGO -->
<br />
<div align="center">
  <a href="https://github.com/SparkyCoder/Gpio_Controller_Api">
    <img src="https://github.githubassets.com/images/modules/logos_page/GitHub-Mark.png" alt="Logo" width="80" height="80">
  </a>

  <h3 align="center">Gpio Controller API</h3>

  <p align="center">
    Kickstart your DIY project
    <br />
    <a href="https://github.com/SparkyCoder/Gpio_Controller_Api/issues/new?labels=bug&template=bug-report---.md">Report Bug</a>
    &middot;
    <a href="https://github.com/SparkyCoder/Gpio_Controller_Api/issues/new?labels=enhancement&template=feature-request---.md">Request Feature</a>
  </p>
</div>



<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgments">Acknowledgments</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project
No coding required. Just follow the <a href="#installation">installation steps</a> to get quick access to gpio info and read / write abilities. 

This project was originally created as a quick way to integrate with [Libre Computer developement boards](https://libre.computer/).
Unlike the Rasberry Pi boards, there's no dedicated code editor or purpose built libraries like WiringPi or Pigpio. The difference in chip architectures adds further complexity. 

This API abstracts the need to understand most underlying hardware, chipset, or drivers your board is using. You can even skip the coding. 
 
Simply use [curl](https://curl.se/), [PostMan](https://www.postman.com/), or any other software that allows you to make REST API calls.


<p align="right">(<a href="#readme-top">back to top</a>)</p>


<!-- GETTING STARTED -->
## Getting Started

Okay, let's get started...

### Prerequisites

The first thing you need to do is ensure you've already installed some version of Linux on your development board. <br /> 
Follow your boards documentation if you haven't already.
<br /><br />
For this tutorial we'll be using a [La Potato AML-S905X-CC](https://hub.libre.computer/t/ubuntu-22-04-jammy-lts-for-libre-computer-boards/20) with Ubuntu 22.4 (Jammy) installed.
<br />If you have the same board and don't mind using a headless version, use the [base-arm64+aml-s905x-cc](https://distro.libre.computer/ci/ubuntu/22.04/ubuntu-22.04.3-preinstalled-base-arm64%2Baml-s905x-cc.img.xz) version.

Next steps:
- Ensure you have curl installed:
```sh
  sudo apt-get install curl
```
- Ensure you have gpiod installed
```sh
  sudo apt-get install gpiod
```

- <b>Optional:</b> Install crontab if you want to automatically run the API on startup. 
```sh
  sudo apt-get install crontab
```

### Installation
1. Boot up your board and log in
2. Run the debian package installation
   ```sh
   sudo curl https://raw.githubusercontent.com/SparkyCoder/Gpio_Controller_Api/refs/heads/main/Installation/install.sh | bash
   ```
3. Navigate to the install folder. 
<br/><b>Don't skip this step!</b> If you do, the API will not work as expected.
   ```sh
   cd /opt/gpio-controller-api-1.1
   ```
4. Run the application
   ```sh
   sudo ./GpioController
   ```
4. <b>Optional:</b> To run the API on startup
   1. Run
      ```sh
      sudo crontab -e
      ```
      2. Then add the following line to the file
      ```sh
      @reboot cd/opt/gpio-controller-api-1.1; ./GpioController
      ```
<br />
Congrats! That's it!

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- USAGE EXAMPLES -->
## Usage
Your API should now be listening for requests on Port 3005 of your developement board.
Using curl or PostMan send an HTTP to your boards ip address using the routes below.<br/>

Endpoints:
- GET /gpios - Returns a list available GPIOs on your board
- GET /gpios/{id} - Coming Soon... - Read the gpio state High (1) or Low (0)
- PATCH /gpios - Change the GPIOs output to High (1) or Low (0)

Examples:
```sh
    curl -X GET "http://192.168.1.144:3005/gpios" 
```

```sh
    curl -X PATCH "http://192.168.1.144:3005/gpios" \
     -H "Content-Type: application/json" \
     -d '[
           {
               "Gpio": 81,
               "Chipset": 1,
               "State": "Low",
               "Options": {
                   "Milliseconds": 1000,
                   "RepeatTimes": 2
               }
           }
         ]'
```

## Advanced Usage

In some cases, you'll want to reach your API from outside your local intranet. <br/>
To do this you'll need to go to your router and add a rule. It's different for each router, but basically, you'll want to allow external traffic through to your board's IP and expose port 3005. Once you do this your API will be accessible via your public IP assigned by your ISP. 
<br/><br/>
If you do this it's is recommended to put Bearer Token Authorization around your endpoints. If you navigate to the [GpioController](https://github.com/SparkyCoder/Gpio_Controller_Api/blob/main/GpioController/Controllers/GpioController.cs) You can see that there are some commented out lines. If you uncomment `[Authorize]` and comment out `[AllowAnonymous]` - Then uncomment the following lines.
This will require users to provide a valid Google JWT. 
<br/> 
<br/>
If you wish to restrict your API to specific users you can uncomment the `IsAuthorized` lines so that it looks like this example:
```csharp
    [Authorize]
    [HttpPatch(Name = "Patch")]
    public IActionResult Patch([FromBody] IEnumerable<GpioSetRequest> patchRequest)
    {
        if (!IsAuthorized())
             return Unauthorized();
        ...
```

Then only users whitelisted in your [AppSettings](https://github.com/SparkyCoder/Gpio_Controller_Api/blob/main/GpioController/appsettings.json) will be allowed to call your APIs. 

<!-- ROADMAP -->
## Roadmap

- [x] add GET /gpios endpoint to list available GPIOs
- [x] Add PATCH /gpios endpoint to Update GPIO state
- [ ] Add GET /gpios/{id} to read GPIO values
- [ ] Add installation for Google JWT authorization via Debian package
- [ ] Add additional chipset architectures
    - [ ] linux-muscl-64
    - [ ] linux-arm

See the [open issues](https://github.com/SparkyCoder/Gpio_Controller_Api/issues) for a full list of proposed features (and known issues).

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- CONTRIBUTING -->
## Contributing

If you have a suggestion that would make this API better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".
Don't forget to give the project a star! Thanks again!

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- LICENSE -->
## License

Distributed under the Unlicense License. See `LICENSE.txt` for more information.

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- CONTACT -->
## Contact

David Kobuszewski - dkob8789@gmail.com

Project Link: [Gpio_Controller_Api](ps://github.com/SparkyCoder/Gpio_Controller_Api)

<p align="right">(<a href="#readme-top">back to top</a>)</p>


<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/sparkycoder/gpio_controller_api.svg?style=for-the-badge
[contributors-url]: https://github.com/SparkyCoder/Gpio_Controller_Api/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/SparkyCoder/Gpio_Controller_Api.svg?style=for-the-badge
[forks-url]: https://github.com/SparkyCoder/Gpio_Controller_Api/network/members
[stars-shield]: https://img.shields.io/github/stars/SparkyCoder/Gpio_Controller_Api.svg?style=for-the-badge
[stars-url]: https://github.com/SparkyCoder/Gpio_Controller_Api/stargazers
[issues-shield]: https://img.shields.io/github/issues/SparkyCoder/Gpio_Controller_Api.svg?style=for-the-badge
[issues-url]: https://github.com/SparkyCoder/Gpio_Controller_Api/issues
[license-shield]: https://img.shields.io/github/license/SparkyCoder/Gpio_Controller_Api.svg?style=for-the-badge
[license-url]: https://github.com/SparkyCoder/Gpio_Controller_Api/blob/master/LICENSE.txt
[product-screenshot]: images/screenshot.png
[Next.js]: https://img.shields.io/badge/next.js-000000?style=for-the-badge&logo=nextdotjs&logoColor=white
[Next-url]: https://nextjs.org/
[React.js]: https://img.shields.io/badge/React-20232A?style=for-the-badge&logo=react&logoColor=61DAFB
[React-url]: https://reactjs.org/
[Vue.js]: https://img.shields.io/badge/Vue.js-35495E?style=for-the-badge&logo=vuedotjs&logoColor=4FC08D
[Vue-url]: https://vuejs.org/
[Angular.io]: https://img.shields.io/badge/Angular-DD0031?style=for-the-badge&logo=angular&logoColor=white
[Angular-url]: https://angular.io/
[Svelte.dev]: https://img.shields.io/badge/Svelte-4A4A55?style=for-the-badge&logo=svelte&logoColor=FF3E00
[Svelte-url]: https://svelte.dev/
[Laravel.com]: https://img.shields.io/badge/Laravel-FF2D20?style=for-the-badge&logo=laravel&logoColor=white
[Laravel-url]: https://laravel.com
[Bootstrap.com]: https://img.shields.io/badge/Bootstrap-563D7C?style=for-the-badge&logo=bootstrap&logoColor=white
[Bootstrap-url]: https://getbootstrap.com
[JQuery.com]: https://img.shields.io/badge/jQuery-0769AD?style=for-the-badge&logo=jquery&logoColor=white
[JQuery-url]: https://jquery.com 

