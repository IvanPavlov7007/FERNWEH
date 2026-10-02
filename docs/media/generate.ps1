param([string]$FFmpeg = 'ffmpeg')

$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
$video = Join-Path $repoRoot 'Promo/FERNWEH_Gameplay1080.mp4'
$banner = Join-Path $repoRoot 'Promo/ITCH_Banner.png'

foreach ($source in @($video, $banner)) {
    if (-not (Test-Path -LiteralPath $source -PathType Leaf)) {
        throw "Missing local media source: $source. See docs/media/README.md."
    }
}
$null = Get-Command $FFmpeg -ErrorAction Stop

$clips = @(
    @{ Name = 'sailing'; Start = '0.5'; Duration = '4' },
    @{ Name = 'coast'; Start = '12'; Duration = '4' }
)
$filter = 'fps=10,scale=480:-2:flags=lanczos,split[a][b];[a]palettegen=max_colors=96:stats_mode=diff[p];[b][p]paletteuse=dither=bayer:bayer_scale=4'

foreach ($clip in $clips) {
    $output = Join-Path $PSScriptRoot ($clip.Name + '.gif')
    & $FFmpeg -hide_banner -loglevel error -y -ss $clip.Start -t $clip.Duration `
        -i $video -filter_complex $filter -an -vsync 0 -loop 0 $output
    if ($LASTEXITCODE -ne 0) {
        throw "FFmpeg failed while generating $output (exit $LASTEXITCODE)."
    }
    Get-Item -LiteralPath $output | Select-Object Name, Length
}

Copy-Item -LiteralPath $banner -Destination (Join-Path $PSScriptRoot 'fernweh.png')
