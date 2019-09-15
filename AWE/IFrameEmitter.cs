namespace AWE {

    public interface IFrameEmitter {

        event DFrameEvent FrameBegin;
        event DFrameEvent FrameIntermediate;
        event DFrameEvent FrameEnd;

    }
}
