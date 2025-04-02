TEMP_DEB="$(mktemp)" &&
echo "HELOOOOOO $TEMP_DB" &&
wget -O "$TEMP_DEB" 'https://raw.githubusercontent.com/SparkyCoder/Gpio_Controller_Api/main/Installation/linux-arm64/gpio-controller-api-1.1.deb' &&
sudo dpkg -i "$TEMP_DEB"
rm -f "$TEMP_DEB"

