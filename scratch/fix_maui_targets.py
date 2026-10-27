csproj_path = "src/Net11Showcase.Maui/Net11Showcase.Maui.csproj"
with open(csproj_path, "r", encoding="utf-8") as f:
    text = f.read()

lines = text.splitlines(keepends=True)
new_lines = []
for line in lines:
    if "<TargetFrameworks>net11.0-android</TargetFrameworks>" in line:
        new_lines.append("        <!-- Target Windows locally on Windows host, multi-target Android/iOS on CI -->\n")
        new_lines.append("        <TargetFrameworks Condition=\"$([MSBuild]::IsOSPlatform('windows'))\">net11.0-windows10.0.19041.0</TargetFrameworks>\n")
        new_lines.append("        <TargetFrameworks Condition=\"!$([MSBuild]::IsOSPlatform('windows'))\">net11.0-android;net11.0-ios;net11.0-maccatalyst</TargetFrameworks>\n")
    elif "Condition=\"!$([MSBuild]::IsOSPlatform('linux'))\"" in line or ("Condition=\"$([MSBuild]::IsOSPlatform('windows'))\"" in line and "<TargetFrameworks>" not in line):
        continue
    else:
        new_lines.append(line)

with open(csproj_path, "w", encoding="utf-8", newline="\r\n") as f:
    f.writelines(new_lines)
print("Updated MAUI csproj!")
