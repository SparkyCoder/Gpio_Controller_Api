<a id="readme-top"></a>
[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![Unlicense License][license-shield]][license-url]
[![LinkedIn][linkedin-shield]][linkedin-url]


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
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#settings">Settings</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project

- Do you want to start using GPIOs on your SBC?
- Don't want to learn about chipsets, architectures, or drivers?
- Want a simple solution that works regardless of manufacturer?

Well, you're in the right place.<br/>
No coding required. Just follow the <a href="#installation">installation steps</a> to get quick access to GPIO functionality.
<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- GETTING STARTED -->
## Getting Started

### Prerequisites

- Have a Linux Distro installed on your board. 
    <details>
  <summary>More Info</summary>
   <br/>Follow your boards documentation if you haven't already. <br/>

    For example, I personally use a [La Potato AML-S905X-CC](https://hub.libre.computer/t/ubuntu-22-04-jammy-lts-for-libre-computer-boards/20) with [Ubuntu 22.4 (Jammy) installed](https://en.wikipedia.org/wiki/Ubuntu_version_history).
<br />If you have the same board and don't mind using a headless version, use the [base-arm64+aml-s905x-cc](https://distro.libre.computer/ci/ubuntu/22.04/ubuntu-22.04.3-preinstalled-base-arm64%2Baml-s905x-cc.img.xz) version.
</details>


<details>
  <summary>Optional Installs</summary>

- Install curl:

```sh
  sudo apt-get install curl
```

- Install crontab if you want to automatically run the API on startup. 

```sh
  sudo apt-get install crontab
```
</details>

### Installation
1. Run the debian package installation
   ```sh
   sudo curl https://raw.githubusercontent.com/SparkyCoder/Gpio_Controller_Api/refs/heads/main/Installation/Install.sh | bash
   ```
<details>
  <summary>Optional Installs</summary>

1. To start the API on reboot:
      ```sh
      sudo crontab -e
      ```
2. Then add the following line to the file
   ```sh
   @reboot cd/opt/gpio-controller-api-1.4; ./GpioController >> /opt/gpio-controller-api-1.4-logs
   ```
</details>

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- USAGE EXAMPLES -->
## Usage
[Postman Documentation](https://documenter.getpostman.com/view/2447338/2sB2cSiPcM)
<details>
  <summary>GET /sbc/chipsets/gpios</summary>

- Description: Get information about all available GPIOs on your board. <br/><br/>

- Example

```sh
    curl -X GET "http://69.203.14.52:3005/sbc/chipsets/gpios" 
```


<b>Important! <br/>
- The ID from the response will correlate to the Linux ID in your SBC documentation. 
- Not all GPIO states can be updated. You'll see an error on the SBC terminal if you attempt this and it cannot be updated.</b>

</details>

<details>
  <summary>GET /sbc/chipsets/{chipsetId}/gpios/{gpioId}</summary>

- Description: Get information about a specific GPIO on your board.


- Example

```sh
    curl -X GET "http://69.203.14.52:3005/sbc/chipsets/1/gpios/81" 
```
</details>

<details>
  <summary>GET /sbc/chipsets/{chipsetId}/gpios/{gpioId}/state</summary>

- Description: Read current state of your GPIO


- Example

```sh
    curl -X GET "http://69.203.14.52:3005/sbc/chipsets/1/gpios/81/state" 
```
</details>

<details>
  <summary>POST /sbc/chipsets/{chipsetId}/gpios/{gpioId}/state/{state}</summary>

- Description: Change the state of a single GPIO
- State: Can be 1, 0, Low, or High.


- Example

```sh
    curl -X POST "http://69.203.14.52:3005/sbc/chipsets/1/gpios/81/state/0" 
```
</details>

<details>
  <summary>POST /sbc/chipsets/gpios/state</summary>

- Description: Allows you to change multiple GPIO states to "Low" or "High" You can also choose to repeat the task or add a delay.

- Example (Cancels all active requests) :

```sh
    curl -X PATCH "http://69.203.14.52:3005/sbc/chipsets/gpios/state" \
     -H "Content-Type: application/json" \
     -d '[]'
```

- Example (Starts a new request):

```sh
    curl -X PATCH "http://69.203.14.52:3005/sbc/chipsets/gpios/state" \
     -H "Content-Type: application/json" \
     -d '[
            {
                "Gpios": [91, 92, 81],
                "Chipset": 1,
                "State": "Low",
                "Options": {
                    "Milliseconds": 30000,
                    "RepeatTimes": 1
                }
            },
            {
                "Gpios": [93],
                "Chipset": 1,
                "State": "High",
                "Options": {
                    "Milliseconds": 500,
                    "RepeatTimes": 10
                }
            }
        ]'
```
</details>


## Settings

To update your API settings, refer to the [AppSettings](https://github.com/SparkyCoder/Gpio_Controller_Api/blob/main/GpioController/appsettings.json) file in your optional installs directory: `/opt/gpio-controller-api-1.4`.

#### Kestral:
- `Kestral:Endpoints:Http:Url` - This is the endpoint your API will bind to. <br/>
    By default it's set to 0:0:0:0:3005. Meaning it will bind to http://localhost:3005 and http://your.ip.address:3005.

#### Authorization:
- `Authorization:Enabled` - allows values "true" or "false". 
  - False: Allows access to any CORS origins and headers. Recommended only for developement. 
  - True: Requires a Google JWT to be authorized. It also requires the following authorization settings to be filled.


- `Authorization:AuthorizedEmails` - Restricts entry to only the following Google users. <br/>
  - Example value: `["yourEmailHere@gmail.com, anotherEmailHere@gmail.com]`



- `Authorization:AuthorizedCorsOrigins` - Restricts CORS origins. For security purposes only requests with the listed origins are allowed through.

#### Filters:

- `Filters:AllowOnlyTheseChipsets` - `GET /sbc/chipsets/gpios` will usually return all <b>Chipsets</b> from your board. Some projects only require a subset of these. Adding IDs here filters results. 
  - Example: `[1]`



- `Filters:AllowOnlyTheseGpios` - `GET /sbc/chipsets/gpios` will usually return all <b>GPIOs</b> from your board. Some projects only require a subset of these. Adding IDs here filters results.
    - Example: `[91, 92, 81,95,80,79,94,93]`


<b>Important!</b><br/>If you expose your IP and Port to the public (By adding a rule to your router / firewall) it is <b>highly recommended</b> to set `Authorization:Enabled` to `true`. Without it, anybody can call your API.
<br/>

<!-- ROADMAP -->
## Roadmap

- [x] add to list available GPIOs
- [x] Add endpoint to Update GPIO state
- [x] Add endpoint to read GPIO values
- [x] Add secure endpoints for Google JWT Auth
- [ ] Add additional chipset architectures
    - [ ] linux-muscl-64
    - [ ] linux-arm

See the [open issues](https://github.com/SparkyCoder/Gpio_Controller_Api/issues) for a full list of proposed features (and known issues).

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- CONTRIBUTING -->
## Contributing

If you have a suggestion that would make this API better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".
<br/><br/><b><h3>Don't forget to give the project a star! Thanks again!</h3><b/>

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- LICENSE -->
## License

 See [LICENSE.txt](https://github.com/SparkyCoder/Gpio_Controller_Api/blob/main/LICENSE) for more information.

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- CONTACT -->
## Contact

David Kobuszewski - dkob8789@gmail.com

Project Link: [Gpio Controller Api](https://github.com/SparkyCoder/Gpio_Controller_Api)

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
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://www.linkedin.com/in/david-kobuszewski-60315428
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

