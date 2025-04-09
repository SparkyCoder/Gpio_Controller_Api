ARCHITECTURE=$(uname -m)

case $ARCHITECTURE in
    x86_64)
        echo "64-bit architecture detected."
        URL="https://github.com/SparkyCoder/PinPanda-API/raw/refs/heads/main/Installation/linux-x64/PinPanda-Api-1.4.deb" 
        ;;
    aarch64)
        echo "ARM 64-bit architecture detected."
        URL="https://github.com/SparkyCoder/PinPanda-API/raw/refs/heads/main/Installation/linux-arm64/PinPanda-Api-1.4.deb"
        ;;
    #armv7l)
    #    echo "ARM 64-bit architecture detected."
    #    URL="https://github.com/SparkyCoder/PinPanda-API/raw/refs/heads/main/Installation/linux-arm/PinPanda-Api-1.4.deb"
    #    ;;
    #armhf)
    #    echo "ARM 64-bit architecture detected."
    #    URL="https://github.com/SparkyCoder/PinPanda-API/raw/refs/heads/main/Installation/linux-arm/PinPanda-Api-1.4.deb"
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
cd /opt/pinpanda-api-1.4 &&
sudo ./PinPanda-Api

