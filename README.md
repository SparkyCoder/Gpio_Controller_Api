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
  <a href="https://github.com/SparkyCoder/PinPanda-API">
    <img src="/images/PinPanda-Api-Logo.jpeg" alt="Logo" width="350" height="350">
  </a>

  <h3 align="center">The Cutest Way to Control Your GPIOs</h3>
    <h3 align="center">https://pinpanda-api.com/</h3>

  <p align="center">
    <br />
    <a href="https://github.com/SparkyCoder/PinPanda-API/issues/new?labels=bug&template=bug-report---.md">Report Bug</a>
    &middot;
    <a href="https://github.com/SparkyCoder/PinPanda-API/issues/new?labels=enhancement&template=feature-request---.md">Request Feature</a>
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
    <li><a href="#online-usage">Online Usage</a></li>
    <li><a href="#local-usage">Local Usage</a></li>
    <li><a href="#architecture">Architecture</a></li>
    <li><a href="#advanced-settings">Settings</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project
PinPanda-API is a easily installable REST API that allows you to interact with GPIOs online at https://pinpanda-api.com/ or locally. No coding required. 
It makes any DIY project a breeze. No more getting involved with chipset architectures or worrying about different board manufacturers. Just dive in and get started fast!


Follow the <a href="#installation">installation steps</a> to get started.
<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- GETTING STARTED -->
## Getting Started

### Prerequisites

- Have an Ubuntu / Debian Distro installed on your board. 
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
   sudo curl https://pinpanda-api.com/Install.sh | bash
   ```



<details>
  <summary>(Optional) Have PinPanda-API launch on Reboot</summary>

1. To start the API on reboot:
      ```sh
      sudo crontab -e
      ```
2. Then add the following line to the file
   ```sh
   @reboot cd/opt/pinpanda-api-1.4; ./PinPanda-Api >> /opt/pinpanda-api-1.4-logs
   ```
</details>

<details>
<summary>(Optional, but Required for Online Control) Grant Access</summary>

1. Go into the appsettings.json file and update the `AuthorizedEmails` value with the email address you'll be logging in with. <br/>
   <img src="/images/step0.png" alt="Logo" width="400" height="550">
2. At this point, the API is only available on your local intranet. To make it accessible from [PinPada-API](https://pinpanda-api.com) you'll need to give it access. To do this login to your router or firewall and add a rule. Allow TCP traffic for the IP of your SBC Development Board and the port the API is running on (Default is 3005). It should look something like this: <br/>
   <img src="/images/step1.png" alt="Logo" width="270" height="600"><br/>


</details>

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- USAGE EXAMPLES -->
## Local-Usage
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

## Online-Usage
1 - Navigate to the [PinPanda-API website](https://pinpanda-api.com)

2 - Login with Google Oauth2.0<br/>

3 - Visit [whatismyipaddress](https://whatismyipaddress.com/) to find and copy your public IP address. This is how [PinPanda-API](https://pinpanda-api.com) will communicate securely with your board.

4 - Enter in that IPv4 address you just copied, followed by the port your API is running on. (Default is 3005)

5 - Add a new request and start interacting with the GPIOs on your board.

6 - Send requests to turn off / on your GPIO pins. Use the red stop button to cancel all active requests. That's it!

<details>
<summary>Step-by-step screenshots</summary>
<img src="/images/step2.png" alt="Logo" width="270" height="600">
<img src="/images/step3.png" alt="Logo" width="270" height="600">
<img src="/images/step4.png" alt="Logo" width="270" height="600">
<img src="/images/step5.png" alt="Logo" width="270" height="600">
<img src="/images/step6.png" alt="Logo" width="270" height="600">
</details>

## Architecture
<img src="/images/architecture.png" alt="Logo" width="675" height="300">


## Advanced-Settings

To update your API settings, refer to the [AppSettings](https://github.com/SparkyCoder/PinPanda-API/blob/main/GpioController/appsettings.json) file in your optional installs directory: `/opt/pinpanda-api-1.4`.

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

### Mappings:

- `Mappings:GpioNames` - `GET /sbc/chipsets/gpios` will usually return the names assigned by the SBC. If you'd like to return custom names in their place, use this setting. 
  - Example: ```[
      {
      "Id": 91,
      "Name": "VCC Power"
      },
      {
        "Id": 92,
        "Name": "Common"
      },
      {
        "Id": 81,
        "Name": "Zone 1"
      },
      {
        "Id": 85,
        "Name": "Zone 2"
      }
]```
  

<b>Important!</b><br/>If you expose your IP and Port to the public (By adding a rule to your router / firewall) it is <b>highly recommended</b> to set `Authorization:Enabled` to `true`. Without it, anybody can call your API.
<br/>

<!-- ROADMAP -->
## Roadmap

- [x] Add endpoint to list available GPIOs
- [x] Add endpoint to Update GPIO state
- [x] Add endpoint to read GPIO values
- [x] Add settings to make usage easier (filter, security, configuration, etc...)
- [x] Add secure endpoints for Google JWT Auth
- [x] Create UI to interface with individual boards
- [x] Add communication between SBC and [PinPanda-API](https://pinpanda-api.com)
- [x] Add documentation
- [ ] Add GPIO mapping on API side to set custom labels 
- [ ] Add additional chipset architectures
    - [ ] linux-muscl-64
    - [ ] linux-arm

See the [open issues](https://github.com/SparkyCoder/PinPanda-API/issues) for a full list of proposed features (and known issues).

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- CONTRIBUTING -->
## Contributing

If you have a suggestion that would make this API better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".
<br/><br/>


Contribute in other ways. Sponserships are solely responsible for keeping this project running. If you found this repo useful please consider becoming a sponsor. 

[![Sponsor](https://img.shields.io/badge/Sponsor-💖-ff69b4?style=for-the-badge)](https://github.com/sponsors/SparkyCoder)


<b><h4>Don't forget to give the project a star! </h3><b/>



<b><h4>Thank you! </h3><b/>

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- LICENSE -->
## License

 See [LICENSE.txt](https://github.com/SparkyCoder/PinPanda-API/blob/main/LICENSE) for more information.

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- CONTACT -->
## Contact

David Kobuszewski - dkob8789@gmail.com

Project Link: [Gpio Controller Api](https://github.com/SparkyCoder/PinPanda-API)

<p align="right">(<a href="#readme-top">back to top</a>)</p>


<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/sparkycoder/PinPanda-Api.svg?style=for-the-badge
[contributors-url]: https://github.com/SparkyCoder/PinPanda-API/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/SparkyCoder/PinPanda-Api.svg?style=for-the-badge
[forks-url]: https://github.com/SparkyCoder/PinPanda-API/network/members
[stars-shield]: https://img.shields.io/github/stars/SparkyCoder/PinPanda-Api.svg?style=for-the-badge
[stars-url]: https://github.com/SparkyCoder/PinPanda-API/stargazers
[issues-shield]: https://img.shields.io/github/issues/SparkyCoder/PinPanda-Api.svg?style=for-the-badge
[issues-url]: https://github.com/SparkyCoder/PinPanda-API/issues
[license-shield]: https://img.shields.io/github/license/SparkyCoder/PinPanda-Api.svg?style=for-the-badge
[license-url]: https://github.com/SparkyCoder/PinPanda-API/blob/master/LICENSE.txt
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

