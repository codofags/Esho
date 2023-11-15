using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public enum TrackType
{
    Audio,
    Video,
}

public class AudioPlayerController : MonoBehaviour
{
    public static AudioPlayerController ins;

    [SerializeField]
    private Transform imageContainer;

    [SerializeField]
    private ScrollRect imageContainerScroll;

    private float cellSize;

    private float imageContainerDesirePosition;

    [SerializeField]
    private float SmoothSpeed = 1;

    public bool canMove = false;

    public Color blueColor;
    public Color whiteColor;

    public string passiveHex = "#969EB3";
    public string activeHex = "#B0C5F3";

    public Sprite playSprite;
    public Sprite pauseSprite;

    public int passiveWidth = 1060;
    public int passiveHeight = 190;

    public int activeWidth = 1060;
    public int activeHeight = 245;

    public Sprite AudioIco;
    public Sprite VideoIco;

   

    void Start()
    {
        ins = this;

        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += VideoSource_loopPointReached;

        audioSource = GetComponent<AudioSource>();


        //SetupPlayList();

        Setup();
    }

   

    private void Setup()
    {
        audioTimeLineSize = audioTimeLine.rect.size.x;
        videoTimeLineSize = videoTimeLine.rect.size.y;

        var imageCount = imageContainer.childCount;

        Debug.Log($"imageCount {imageCount}");

        var gridLayout = imageContainer.GetComponent<GridLayoutGroup>();

        var gridSpacing = gridLayout.spacing;
        var gridCellSize = gridLayout.cellSize;
        var gridPadding = gridLayout.padding;

        //var desireWidth = imageCount * gridCellSize.x + (imageCount - 1) * gridSpacing.x + gridPadding.left+ gridPadding.right;

        //imageContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(desireWidth, 0);        

        cellSize = (imageCount * gridCellSize.x + (imageCount) * gridSpacing.x) / imageCount;

        Debug.Log($"cellSize {cellSize}");

        ButtonManager.ins.OnSwitchLanguage += OnSwitchLanguage;
    }

   

    //public void OnImageContainerDrop()
    //{
    //    var x = imageContainer.localPosition.x ;

    //    x = x - 350 - 30;

    //    var cell = (int)(x / cellSize);

    //    SetPlayListPosition(cell);
    //}

    [SerializeField] private TextMeshProUGUI imageTrackName;

    private ImageTrackController currentImageController;


    public void PreviousPlayListPosition()
    {
        currentTackIndex -= 1;

        if (currentTackIndex < 0)
        {
            currentTackIndex = 0;
        }

        //audioHorizontalDrag.currentTrack = currentTackIndex;

        audioHorizontalDrag.SelectTrack(currentTackIndex);

        PlayTrack(trackControllers[currentTackIndex]);      
    }

    public void NextPlayListPosition()
    {
        currentTackIndex += 1;

        if (currentTackIndex > trackControllers.Count-1)
        {
            currentTackIndex = trackControllers.Count - 1;
        }

        //audioHorizontalDrag.currentTrack = currentTackIndex;

        PlayTrack(trackControllers[currentTackIndex]);

        audioHorizontalDrag.SelectTrack(currentTackIndex);       
    }

    public bool timeLinePinIsLocked=false;

    public void StartUnlockTimeLinePin()
    {
        StartCoroutine(UnlockTimeLinePin());
    }

    IEnumerator UnlockTimeLinePin()
    {
        yield return new WaitForSeconds(0.5f);

        timeLinePinIsLocked = false;
    }

    void Update()
    {
        //if (canMove)
        //{
        //    //var currentPosition = imageContainer.localPosition;
        //    //var desirePosition = new Vector3(imageContainerDesirePosition, 0, 0);

        //    //imageContainer.localPosition = Vector3.Lerp(currentPosition, desirePosition, Time.deltaTime * SmoothSpeed);
        //}

        if (!timeLinePinIsLocked)
        {
            UpdateAudioPlayerTimeLine();

            UpdateVideoPlayerTimeLine();
        }       

        CheckAudioClipEnd();
    }

    private void CheckAudioClipEnd()
    {
        if (audioSource.clip != null)
        {
            if (audioSource.time >= audioSource.clip.length-0.1f)
            {
                audioSource.Stop();
                audioSource.time = 0;

                EnbleReplay();
                Debug.Log("audio end");
            }
        }
    }

    private void EnbleReplay()
    {
        //меняем кнопки на плей
        ChangePlayerButtonsToPlay();
        currentTrackControler.SetPlay();
    }

    internal void SetTimeLine(float progress)
    {
        if (progress < 0) progress = 0;
        if (progress >= 0.99f) progress = 0.99f;

        if (audioSource.isPlaying)
        {
            audioSource.time = audioSource.clip.length * progress;
        }
        if(!audioSource.isPlaying && audioSource.clip != null)
        {
            PlayTrack(currentTrackControler);

            audioSource.time = audioSource.clip.length * progress;
        }

        if (videoPlayer.isPlaying)
        {
            videoPlayer.time = videoPlayer.clip.length * progress;
        }
        if (!videoPlayer.isPlaying && videoPlayer.clip != null)
        {
            PlayTrack(currentTrackControler);
            videoPlayer.time = videoPlayer.clip.length * progress;
        }
    }

    //audio
    [SerializeField] private RectTransform audioTimeLine;
    private float audioTimeLineSize;
    [SerializeField] private RectTransform audioTimePin;
    [SerializeField] private TextMeshProUGUI audioTrackTime;
    [SerializeField] private TextMeshProUGUI audioTrackDuration;   

    private void UpdateAudioPlayerTimeLine()
    {
        if (audioSource.isPlaying)
        {
            //timeLineStep = timeLineSize / audioSource.clip.length;

            var trackPosition = audioSource.time / audioSource.clip.length;
            //Debug.Log(timeLineStep);

            audioTimePin.anchoredPosition = new Vector2(trackPosition * audioTimeLineSize, 0);

            audioTrackTime.text = GetTimeFromSeconds( audioSource.time);
            audioTrackDuration.text = GetTimeFromSeconds(audioSource.clip.length);
        }
    }

    //video
    [SerializeField] private RectTransform videoTimeLine;
    private float videoTimeLineSize;
    [SerializeField] private RectTransform videoTimePin;
    [SerializeField] private TextMeshProUGUI videoTrackTime;
    [SerializeField] private TextMeshProUGUI videoTrackDuration;

    private void UpdateVideoPlayerTimeLine()
    {
        if (videoPlayer.isPlaying)
        {
            var trackPosition = (float)(videoPlayer.time / videoPlayer.clip.length);

            videoTimePin.anchoredPosition = new Vector2(0,-trackPosition * videoTimeLineSize);

            videoTrackTime.text = GetTimeFromSeconds((float)videoPlayer.time);
            videoTrackDuration.text = GetTimeFromSeconds((float)videoPlayer.clip.length);

            audioTimePin.anchoredPosition = new Vector2(trackPosition * audioTimeLineSize, 0);

            audioTrackTime.text = GetTimeFromSeconds((float)videoPlayer.time);
            audioTrackDuration.text = GetTimeFromSeconds((float)videoPlayer.clip.length);
        }
    }

    //перемотка текущего трека назад на 10 сек
    public void BackwardTimeLine()
    {
        if (audioSource.isPlaying)
        {
            var currentClipTime = audioSource.time;

            currentClipTime -= 10;

            if (currentClipTime < 0) currentClipTime = 0;

            audioSource.time = currentClipTime;
        }

        
        if (videoPlayer.isPlaying)
        {
            var currentClipTime = videoPlayer.time;

            currentClipTime -= 10;

            if (currentClipTime < 0) currentClipTime = 0;

            videoPlayer.time = currentClipTime;
        }       
    }

    //перемотка текущего трека вперед на 10 сек
    public void ForwardTimeLine()
    {
        if (audioSource.isPlaying)
        {
            var currentClipTime = audioSource.time;

            currentClipTime += 10;

            if (currentClipTime > audioSource.clip.length) currentClipTime = audioSource.clip.length - 0.1f;

            audioSource.time = currentClipTime;
        }

        if (videoPlayer.isPlaying)
        {
            var currentClipTime = videoPlayer.time;

            currentClipTime += 10;

            if (currentClipTime > videoPlayer.clip.length) currentClipTime = videoPlayer.clip.length - 0.1f;

            videoPlayer.time = currentClipTime;         
        }
    }

    //private void SetTimeLine(float newTime)
    //{
    //    audioSource.time = newTime;
    //}

    private string GetTimeFromSeconds(float seconds)
    {
        var result = "";

        int resultMinutes = (int)(seconds / 60);
        var resultSeconds = seconds - resultMinutes * 60;

        if (resultMinutes == 0)
        {
            result = "00:";
        }
        else if(resultMinutes>0 && resultMinutes < 10)
        {
            result = $"0{resultMinutes}:";
        }

        if (resultSeconds < 10)
        {
            result += "0";
        }

        result += resultSeconds.ToString("f0");

        return result;
    }

    #region Play List

    [SerializeField]
    private GameObject TrackControllerPrefab;

    [SerializeField]
    private Transform TrackListContainer;

    //[SerializeField]
    public List<TrackController> trackControllers = new List<TrackController>();

    [SerializeField] private TextMeshProUGUI mainCaption;
    [SerializeField] private GameObject separator;
    [SerializeField] private GameObject customCaption;

    [SerializeField] private Transform pointsContainer;

    public HorizontalDrag audioHorizontalDrag;

    private void ClearPlayList()
    {
        trackIndex = 0;

        //
        contentConfigs.Clear();

        //чистим конфиги треков
        trackConfigs.Clear();

        //чистим квадратные плашки в горизонтальном плейлисте
        for (int i = imageContainer.childCount - 1; i >= 0; i--)
        {
            var child = imageContainer.GetChild(i).gameObject;
            ContentManager.inst.MoveToTrash(child);
        }
        imageTrackControllers.Clear();

        //чистим горизонтальные плашки в вертикальном плейлисте
        for (int i = TrackListContainer.childCount - 1; i >= 0; i--) 
        {
            var child = TrackListContainer.GetChild(i).gameObject;
            ContentManager.inst.MoveToTrash(child);
        }
        trackControllers.Clear();
    }   

    //public void SetupPlayList(PointConfig point)
    //{
    //    var path = point.path;
    //    SetupPlayList(path);
    //}

    //public void SetupPlayList(GPS_Point_Config point)
    //{
    //    var path = point.path;
    //    SetupPlayList(path);
    //}

    [SerializeField] private TextTranslation audiogidTranslation;
    //public void SetupPlayList(PathConfig path)
    //{
    //    ClearPlayList();

    //    var gpsPathName = TranslateHelper.ins.GetText(path.translation);

    //    //оформляем заголовок плейлиста. название маршрута
    //    var audiogidText = TranslateHelper.ins.GetText(audiogidTranslation);
    //    mainCaption.text = gpsPathName + $"\n<size=35>\n</size><color=#969EB3><size=45>{audiogidText}</size></color>";

    //    //находим все точки на маршруте
    //    var points = path.points;

    //    //вставляем пустышку в плейлисте
    //    AddListStarter();

    //    //перебираем сценарии в каждой точке на маршруте
    //    for (int i = 0; i < points.Count; i++)
    //    {
    //        //добавить сепаратор 
    //        if (i > 0)
    //        {               
    //            AddSeparator();
    //        }

    //        //добавляем кастомную шапку
    //        try
    //        {
    //            var caption = TranslateHelper.ins.GetText(points[i].translation);
    //            AddCustomCaption(caption);
    //        }
    //        catch (Exception e)
    //        {
    //            Debug.Log(e);
    //        }

    //        //ищем все треки на точке
    //        var scenarios = points[i].scenarios;

    //        if (scenarios == null) continue;            

    //        for (int a = 0; a < scenarios.Count; a++)
    //        {
    //            AddTrackToPlayList(a + 1, scenarios[a]);
    //        }

    //    }

    //    AddListFinalizator();

    //    Setup();      

    //    audioHorizontalDrag.Setup();

    //    //если сейчас что-то проигрывается
    //    if (currentTrackControler != null)
    //    {
    //        if(currentTrackControler.trackType== TrackType.Audio)
    //        {
    //            if (audioSource.isPlaying)
    //            {
    //                foreach(var t in trackControllers)
    //                {
    //                    if (t.audioClip == currentTrackControler.audioClip)
    //                    {
    //                        Debug.Log("audio mutch");
    //                        var i = t.trackIndex;

    //                        trackControllers[i].SetActive();
    //                        imageTrackControllers[i].Unlock();

    //                        currentTrackControler = t;
    //                    }
    //                }
    //            }
    //        }

    //        if(currentTrackControler.trackType == TrackType.Video)
    //        {
    //            if (videoSource.isPlaying)
    //            {
    //                foreach (var t in trackControllers)
    //                {
    //                    if (t.videoClip == currentTrackControler.videoClip)
    //                    {

    //                        var i = t.trackIndex;

    //                        trackControllers[i].SetActive();
    //                        imageTrackControllers[i].Unlock();

    //                        currentTrackControler = t;


    //                        Debug.Log("video mutch "+i);

    //                        //Debug.Log(trackControllers[i]);

    //                        //Debug.Log(imageTrackControllers[i]);
    //                    }
    //                }
    //            }
    //        }

    //    }

    //    else
    //    {
    //        if (trackControllers.Count > 0)
    //        {
    //            var firstTrack = trackControllers[0];
    //            SetupPlayerControllerTrackButton(firstTrack);
    //            //currentTrackControler = firstTrack;
    //            PlayTrack(firstTrack);
    //        }
    //    }


    //    ButtonManager.ins.PlayerController.SetActive(true);
    //}
    private void OnSwitchLanguage(object sender, EventArgs e)
    {
        if (_currentPoint == null) return;

        var repeatAudio = false;

        audioSource.Stop();
        audioSource.clip = null;

        videoPlayer.Stop();
        videoPlayer.clip = null;

        if (currentTrackControler != null && audioSource.isPlaying ||
           currentTrackControler != null && videoPlayer.isPlaying)
        {
            repeatAudio = true;
        }

        SetupPlayList(_currentPoint);

        if (repeatAudio)
        {
            PlayTrack(currentTrackControler);
        }
    }

    private GPS_Point_Config _currentPoint;
    public void SetupPlayList(GPS_Point_Config point, bool playFirstTrack = true)
    {
        _currentPoint = point;

        ClearPlayList();

        var gpsPathName = TranslateHelper.ins.GetText(point.pointName);

        //оформляем заголовок плейлиста. название маршрута
        var audiogidText = TranslateHelper.ins.GetText(audiogidTranslation);
        mainCaption.text = gpsPathName + $"\n<size=35>\n</size><color=#969EB3><size=45>{audiogidText}</size></color>";     

        //вставляем пустышку в плейлисте
        AddListStarter();

        //перебираем сценарии в каждой точке на маршруте
        //for (int i = 0; i < points.Count; i++)
        //{
        var pointCount = 0;

        //перебираем весь контент в точке и добавляем из него треки, если они есть
        int trackNumber = 1;
        //foreach(var p in points)
        {
            //добавить сепаратор 
            if (pointCount > 0)
            {
                AddSeparator();
            }

            //добавляем кастомную шапку
            try
            {
                var caption = TranslateHelper.ins.GetText(point.pointName);
                AddCustomCaption(caption);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

            foreach (var c in point.content)
            {
                if (c is Content_Text_Config)
                {
                    var subContent = ((Content_Text_Config)c).subContent;

                    //Debug.Log($"subContent {subContent.Count}");

                    foreach (var sc in subContent)
                    {
                        if (sc is Content_Audio_Config || sc is Content_Video_Config)
                        {
                            AddTrackToPlayList(trackNumber++, sc);
                        }
                    }
                }
            }
        }
        //}

        AddListFinalizator();

        Setup();

        audioHorizontalDrag.Setup();

        //если сейчас что-то проигрывается
        if (currentTrackControler != null)
        {
            if (currentTrackControler.content is Content_Audio_Config)
            {
                if (audioSource.isPlaying)
                {
                    foreach (var t in trackControllers)
                    {
                        if(t.content == currentTrackControler.content)
                        {
                            Debug.Log("audio mutch");
                            var i = t.trackIndex;

                            trackControllers[i].SetActive();
                            imageTrackControllers[i].Unlock();

                            currentTrackControler = t;
                        }

                        //if (t.audioClip == currentTrackControler.audioClip)
                        //{
                        //    Debug.Log("audio mutch");
                        //    var i = t.trackIndex;

                        //    trackControllers[i].SetActive();
                        //    imageTrackControllers[i].Unlock();

                        //    currentTrackControler = t;
                        //}
                    }
                }
            }

            if (currentTrackControler.content is Content_Video_Config)
            {
                if (videoPlayer.isPlaying)
                {
                    foreach (var t in trackControllers)
                    {
                        if(t.content == currentTrackControler.content)
                        {
                            var i = t.trackIndex;

                            trackControllers[i].SetActive();
                            imageTrackControllers[i].Unlock();

                            currentTrackControler = t;
                        }

                        //if (t.videoClip == currentTrackControler.videoClip)
                        //{

                        //    var i = t.trackIndex;

                        //    trackControllers[i].SetActive();
                        //    imageTrackControllers[i].Unlock();

                        //    currentTrackControler = t;


                        //    Debug.Log("video mutch " + i);

                        //    //Debug.Log(trackControllers[i]);

                        //    //Debug.Log(imageTrackControllers[i]);
                        //}
                    }
                }
            }

        }

        else
        {
            if ( trackControllers.Count > 0)
            {
                var firstTrack = trackControllers[0];
                SetupPlayerControllerTrackButton(firstTrack);
                currentTrackControler = firstTrack;
                playerControllerTrackName.text = firstTrack.trackName;

                if (playFirstTrack) PlayTrack(firstTrack);
            }
        }

        Debug.Log($"{imageTrackControllers.Count} {trackConfigs.Count} {trackControllers.Count}");

        ButtonManager.ins.PlayerController.SetActive(true);
    }

    private void AddSeparator()
    {
        var newSeparator = Instantiate(separator, TrackListContainer);
    }

    [SerializeField] private GameObject listFinalizator;

    private void AddListFinalizator()
    {
        var newListFinalizator = Instantiate(listFinalizator, TrackListContainer);
    }

    [SerializeField] private GameObject listStarter;

    private void AddListStarter()
    {
        var newListStarter = Instantiate(listStarter, TrackListContainer);
    }

    private void AddCustomCaption(string caption)
    {
        var newCustomCaption_GO = Instantiate(customCaption, TrackListContainer);
        var newCustomCaption_SC = newCustomCaption_GO.GetComponent<TextMeshProUGUI>();

        newCustomCaption_SC.text = caption;
    }

    private int trackIndex = 0;

    private List<TrackConfig> trackConfigs = new List<TrackConfig>();

    //int testimagecounter = 0;

    //private void AddTrackToPlayList(int number, ArScenario scenario)
    //{
    //    scenario.track.Setup();
    //    trackConfigs.Add(scenario.track);

    //    var trackController_GO = Instantiate(TrackControllerPrefab, TrackListContainer);        

    //     var trackControler_SC = trackController_GO.GetComponent<TrackController>();
    //    trackControllers.Add(trackControler_SC);

    //    trackControler_SC.Assign(trackIndex++, number, scenario);

    //    //добавляем контроллер трека
        
    //    //Debug.Log(testimagecounter++);
    //    AddImageTack(number, trackControler_SC, scenario.track);
    //}

    public List<Content_Config> contentConfigs = new List<Content_Config>();
    private void AddTrackToPlayList(int number, Content_Config content)
    {
        //Debug.Log($"{content.name}");

        contentConfigs.Add(content);

        //scenario.track.Setup();
        //trackConfigs.Add(scenario.track);

        var trackController_GO = Instantiate(TrackControllerPrefab, TrackListContainer);

        var trackControler_SC = trackController_GO.GetComponent<TrackController>();
        trackControllers.Add(trackControler_SC);

        trackControler_SC.Assign(trackIndex++, number,content);

        AddImageTack(number, trackControler_SC, content);
    }

    [SerializeField] private GameObject imageTrackController;
    [SerializeField] private Sprite defaultTrackIco;

    private List<ImageTrackController> imageTrackControllers = new List<ImageTrackController>();

    //private void AddImageTack(int number, TrackController trackControler, TrackConfig trackConfig)
    //{
    //    //определить аудио или видео

    //    var newImageTrackController_GO = Instantiate(imageTrackController, imageContainer);
    //    var newImageTrackController_SC = newImageTrackController_GO.GetComponent<ImageTrackController>();

    //    imageTrackControllers.Add(newImageTrackController_SC);

    //    newImageTrackController_SC.Assign(defaultTrackIco, number, trackControler, trackConfig);
    //}

    private void AddImageTack(int number, TrackController trackControler, Content_Config content)
    {
        //определить аудио или видео

        var newImageTrackController_GO = Instantiate(imageTrackController, imageContainer);
        var newImageTrackController_SC = newImageTrackController_GO.GetComponent<ImageTrackController>();

        imageTrackControllers.Add(newImageTrackController_SC);

        newImageTrackController_SC.Assign(defaultTrackIco, number, trackControler, content);
    }

    [SerializeField]
    private VerticalLayoutGroup playListLayoutGroup;

    public void UpdatePlaListLayoutGroup()
    {
       // playListLayoutGroup.CalculateLayoutInputVertical();

        playListLayoutGroup.enabled = false;
        playListLayoutGroup.enabled = true;
    }

    #endregion

    public void ResetTracks()
    {
        foreach (var t in trackControllers)
        {
            t.SetPassive();
        }
    }

    public void ResetImageTrackControllers()
    {
        foreach (var t in imageTrackControllers)
        {
            t.Lock();
        }
    }

   

    private AudioSource audioSource;
    //private VideoPlayer videoSource;
    public TrackController currentTrackControler;
    public int currentTackIndex;

    //private TrackController[] currentPointPlayList;

    public void PlayTrack(Content_Config content)
    {
        var imageTrackController = imageTrackControllers.Find(x => x.content == content);
        var trackController = imageTrackController.trackControler;

        PlayTrack(trackController);
    }

    public void PlayTrack(TrackController  trackController)
    {
        ResetTracks();
        ResetImageTrackControllers();

        audioHorizontalDrag.SelectTrack(trackController.trackIndex);

        var imageTrackController = imageTrackControllers[trackController.trackIndex];
        imageTrackController.Unlock();

        trackController.SetActive();

        //если сейчас пролигрывается какой-то трек, то останавливаем его
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.clip = null;
        }
        if (videoPlayer.isPlaying)
        { 
            videoPlayer.Stop();
            videoPlayer.clip = null;
        }

        currentTrackControler = trackController;

        currentTackIndex = trackController.trackIndex;

        //Debug.Log(currentTackIndex);

        //меняем спрайты на всех кнопках плеера
        ChangePlayerButtonsToPause();

        //меняем состояние трека в плейлисте
        trackController.SetActive();

        //меняем картинку в мини плеере
        //playerControllerTrackIco.sprite = trackController.content.;

        //меняем текст в мини плеере
        playerControllerTrackName.text = trackController.trackName;

        //настраиваем в миниплеере переход на большой плеер
        SetupPlayerControllerTrackButton(trackController);

        //воспроизводим клип
        //if (trackController.trackType == TrackType.Audio)
        if (trackController.content is Content_Audio_Config)
        {
            playerControllerVideoImage.gameObject.SetActive(false);

            audioSource.clip = trackController.audioClip;

            audioSource.Play();

            audioSource.time = 0;

            playerControllerTrackIco.sprite = ((Content_Audio_Config)trackController.content).ico;
        }
        else
        {
            //ButtonManager.ins.OpenVideoPlayer();

            videoTexture = new RenderTexture(688, 388, 24);

            ResizeVideoTexture();

            //if(currentTrackControler!= trackController)
            {
                videoPlayer.clip = trackController.videoClip;

                //688 388
                videoPlayer.targetTexture = videoTexture;
                imageTrackController.videoImage.texture = videoTexture;

                //audioSource.clip = ((Content_Video_Config) trackController.content)._audio;
                //audioSource.Play();

                videoPlayer.Play();
            }
            //ResetTracks();
        }

        
    }

    public void ResizeVideoTexture()
    {
        var videoContainerSize = videoTextureContainer.rect.size;

        videoPlayerImage.GetComponent<RectTransform>().sizeDelta = new Vector2(videoContainerSize.y, videoContainerSize.x);

        playerControllerVideoImage.texture = videoTexture;
        videoPlayerImage.texture = videoTexture;

        playerControllerVideoImage.gameObject.SetActive(true);
    }

    private void VideoSource_loopPointReached(VideoPlayer source)
    {
        EnbleReplay();
        Debug.Log("video end");
    }

    //miniPlayer
    public void PlayPauseButton()
    {
        if (audioSource.clip != null )
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
                ChangePlayerButtonsToPlay();

                currentTrackControler.SetPlay();
            }
            else
            {
                audioSource.Play();
                ChangePlayerButtonsToPause();

                currentTrackControler.SetPause();
            }
        }

        if (videoPlayer.clip != null )
        {
            if (videoPlayer.isPlaying)
            {
                videoPlayer.Pause();
                //audioSource.Pause();
                ChangePlayerButtonsToPlay();

                currentTrackControler.SetPlay();
            }
            else
            {
                videoPlayer.Play();
                //audioSource.Play();
                ChangePlayerButtonsToPause();

                currentTrackControler.SetPause();
            }
        }

        if (currentTrackControler != null && videoPlayer.clip == null && audioSource.clip == null) 
        {
            PlayTrack(currentTrackControler);
        }
    }

    [SerializeField] private Image playerControllerTrackIco;
    [SerializeField] private TextMeshProUGUI playerControllerTrackName;
    [SerializeField] private Button playerControllerTrackButton;  

    private void SetupPlayerControllerTrackButton(TrackController trackController)
    {
        playerControllerTrackButton.onClick.RemoveAllListeners();

        playerControllerTrackButton.onClick.AddListener(() => ButtonManager.ins.OpenAudioPlayer(trackController.trackIndex));
    }

    [SerializeField] private Image playerControllerButtonImage;
    [SerializeField] private Image albumControllerButtonImage;
    [SerializeField] private Image videoControllerButtonImage;

    private void ChangePlayerButtonsToPause()
    {
        //mini
        playerControllerButtonImage.sprite = pauseSprite;
        //horizont
        albumControllerButtonImage.sprite = pauseSprite;
        //video
        videoControllerButtonImage.sprite = pauseSprite;
    }

    private void ChangePlayerButtonsToPlay()
    {
        //mini
        playerControllerButtonImage.sprite = playSprite;
        //horizont
        albumControllerButtonImage.sprite = playSprite;
        //video
        videoControllerButtonImage.sprite = playSprite;
    }

    [Header("video")]
    public VideoPlayer videoPlayer;   
    [SerializeField]    private RectTransform videoTextureContainer;
    public RenderTexture videoTexture;
    [SerializeField] private RawImage playerControllerVideoImage;
    [SerializeField] private RawImage videoPlayerImage;

    [SerializeField] private VideoClip defaultVideoClip;
    public Sprite defaultVideoImage;

    public void StopPlayer()
    {
        if (audioSource.clip != null)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
                ChangePlayerButtonsToPlay();

                currentTrackControler.SetPlay();
            }
        }

        if (videoPlayer.clip != null)
        {
            if (videoPlayer.isPlaying)
            {
                videoPlayer.Pause();
                //audioSource.Pause();
                ChangePlayerButtonsToPlay();

                currentTrackControler.SetPlay();
            }
        }
    }


}
