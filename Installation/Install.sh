ARCHITECTURE=$(uname -m)

case $ARCHITECTURE in
    x86_64)
        echo "64-bit architecture detected."
        URL="https://github.com/SparkyCoder/Gpio_Controller_Api/raw/refs/heads/main/Installation/linux-x64/gpio-controller-api-1.4.deb" 
        ;;
    aarch64)
        echo "ARM 64-bit architecture detected."
        URL="https://github.com/SparkyCoder/Gpio_Controller_Api/raw/refs/heads/main/Installation/linux-arm64/gpio-controller-api-1.4.deb"
        ;;
    #armv7l)
    #    echo "ARM 64-bit architecture detected."
    #    URL="https://github.com/SparkyCoder/Gpio_Controller_Api/raw/refs/heads/main/Installation/linux-arm/gpio-controller-api-1.4.deb"
    #    ;;
    #armhf)
    #    echo "ARM 64-bit architecture detected."
    #    URL="https://github.com/SparkyCoder/Gpio_Controller_Api/raw/refs/heads/main/Installation/linux-arm/gpio-controller-api-1.4.deb"
    #    ;;
      
      
    *)
        echo "Unknown architecture: $ARCHITECTURE"
        ;;
esac

sudo apt-get install gpiod &&
TEMP_DEB="$(mktemp)" &&
wget -O "$TEMP_DEB" "$URL" &&
sudo dpkg -i "$TEMP_DEB" &&
rm -f "$TEMP_DEB" &&
cd /opt/gpio-controller-api-1.4 &&
sudo ./GpioController

